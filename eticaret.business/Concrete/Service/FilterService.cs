using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Category;
using eticaret.data.Abstract.Filter;
using eticaret.data.Abstract.Product;
using eticaret.entity.Category;
using eticaret.entity.EntityRefrences.FilterReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Filter;
using eticaret.entity.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class FilterService : IFilterService
    {
        private readonly IFilterBoxRepository _filterBoxRepository;
        private readonly ICategoryService _categoryService;
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IConfiguration _configuration;
        private readonly IFilterRepository _filterRepository;
        private readonly ICategoryRepository _categoryRepository;

        public FilterService(IFilterBoxRepository filterBoxRepository,
                             ICategoryService categoryService,
                             IProductRepository productRepository,
                             IProductImageRepository productImageRepository,
                             IConfiguration configuration,
                             IFilterRepository filterRepository,
                             ICategoryRepository categoryRepository)
        {
            _filterBoxRepository = filterBoxRepository;
            _categoryService = categoryService;
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _configuration = configuration;
            _filterRepository = filterRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<FilterBox>> GetFilterByCategoryAsync(List<string> categoryIds)
        {
            List<FilterBox?> result = new();
            List<FilterBox> allFilters = _filterBoxRepository.Table.AsNoTracking().Include(fb => fb.Categories).Include(fb => fb.Filters).ToList();
            foreach(FilterBox filterBox in allFilters)
            {
                foreach(Category category in filterBox.Categories)
                {
                    if (categoryIds.Contains(category.Id.ToString()))
                    {
                        FilterBox acFilterBox = new()
                        {
                            FilterBoxTitle = filterBox.FilterBoxTitle,
                            Id = filterBox.Id,
                            Filters = filterBox.Filters
                        };
                        if (result.Where(r => r.Id == filterBox.Id).Count() == 0){
                            result.Add(acFilterBox);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<FilterBox>> GetFilterByCategoryIdAsync(string categoryId)
        {
            Category targetCategory = await _categoryRepository.GetByIdAsync(categoryId);
            List<FilterBox> filteredFilters = _filterBoxRepository.Table.AsNoTracking().Include(fb => fb.Categories).Include(fb => fb.Filters).Where(fb => fb.Categories.Contains(targetCategory)).ToList();
            return filteredFilters;
        }

        public async Task<FilterEditModel> GetForEditById(string id)
        {
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Categories).Include(fb => fb.Filters).FirstOrDefaultAsync(fb => fb.Id.ToString() == id);
            List<string> filterIds = filterBox.Filters.Select(f => f.Id.ToString()).ToList();
            List<string> categoryIds = filterBox.Categories.Select(c => c.Id.ToString()).ToList();
            List<Product> products = _productRepository.Table.Include(p => p.ProductImages).Include(p => p.Filters).Where(p => p.Filters.Where(f => filterIds.Contains(f.Id.ToString())).ToList().Count > 0).ToList();
            List<ProductListModel> productsListModels = await ProductToListModelRange(products);
            return new()
            {
                Filters = filterBox.Filters.ToList(),
                FilterBoxTitle = filterBox.FilterBoxTitle,
                TopCategories = _categoryService.GetAllWithFullDeep(),
                Categories = filterBox.Categories.ToList(),
                Id = filterBox.Id.ToString(),
                Products = productsListModels,
                CategoryIds = categoryIds
            };
        }
        public async Task<List<Filter>> GetFiltersByFilterBoxId(string filterBoxId)
        {
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Filters).FirstOrDefaultAsync(fb => fb.Id.ToString() == filterBoxId);
            return filterBox.Filters.ToList();
        }
        public async Task<bool> UpdateFilterName(string filterId, string filterName)
        {
            Filter filter = await _filterRepository.Table.FirstOrDefaultAsync(f => f.Id.ToString() == filterId);
            filter.UpdateDate = DateTime.Now;
            filter.FilterTitle = filterName;
            _filterRepository.Update(filter);
            await _filterRepository.SaveAsync();
            return true;
        }
        public async Task<bool> AddNewFilterToFilterBox(string filterBoxId, string filterName)
        {
            Guid id = Guid.NewGuid();
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Filters).FirstOrDefaultAsync(fb => fb.Id.ToString() == filterBoxId);
            await _filterRepository.AddAsync(new()
            {
                Id = id,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                FilterTitle = filterName,
            });
            await _filterRepository.SaveAsync();
            var filter = await _filterRepository.GetByIdAsync(id.ToString());
            filterBox.Filters.Add(filter);
            _filterBoxRepository.Update(filterBox);
            await _filterBoxRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteFilterFromFilterBox(string filterBoxId, string filterId)
        {
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Filters).FirstOrDefaultAsync(fb => fb.Id.ToString() == filterBoxId);
            Filter filter = await _filterRepository.Table.FirstOrDefaultAsync(f => f.Id.ToString() == filterId);
            filterBox.Filters.Remove(filter);
            _filterRepository.Table.Remove(filter);
            await _filterRepository.SaveAsync();
            return true;
        }
        public async Task<FilterDetailsModel> GetFilterDetailsModelById(string filterId)
        {
            Filter filter = await _filterRepository.Table.Include(f => f.Products).ThenInclude(p => p.ProductImages).FirstOrDefaultAsync(f => f.Id.ToString() == filterId);
            List<ProductListModel> products = await ProductToListModelRange(filter.Products.ToList());
            return new() { FilterTitle = filter.FilterTitle, FilterId = filter.Id.ToString(), Products = products };
        }
        public async Task<bool> RemoveProductFromFilter(string filterId, string productId)
        {
            Filter filter = await _filterRepository.Table.Include(f => f.Products).FirstOrDefaultAsync(f => f.Id.ToString() == filterId);
            Product product = await _productRepository.GetByIdAsync(productId);
            filter.Products.Remove(product);
            _filterRepository.Update(filter);
            await _filterRepository.SaveAsync();
            return true;
        }
        private async Task<List<ProductListModel>> ProductToListModelRange(List<Product> products)
        {
            var result = new List<ProductListModel>();
            foreach (Product product in products)
            {
                result.Add(await ProductToListModel(product));
            }
            return result;
        }
        private async Task<ProductListModel> ProductToListModel(Product product)
        {
            ProductListModel productListModel = new()
            {
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock,
                Price = product.Price,
                Id = product.Id,
                Url = product.Url,
            };
            if (product.ProductImages != null && product.ProductImages.Count > 0)
            {
                product.ProductImages = product.ProductImages.OrderBy(pi => pi.Index).ToList();
                string mainImageId = product.ProductImages.ToList()[0].Id.ToString();
                var mainImage = await _productImageRepository.GetByIdAsync(mainImageId);
                productListModel.MainImageUrl = product.ProductImages.ToList()[0] != null ? $"{_configuration["StoragePaths:Azure"]}{mainImage.Path}" : null;
            }
            else
            {
                string mainImageId = product.MainImageId.ToString();
                var mainImage = await _productImageRepository.GetByIdAsync(mainImageId);
                productListModel.MainImageUrl = $"{_configuration["StoragePaths:Azure"]}{mainImage.Path}";
            }
            //productListModel.ImageUrl = product.ProductImages?.Count > 0 ? $"{_configuration["StoragePaths:Azure"]}" + $"{product.ProductImages.ToList()[0].Path}" : null;
            productListModel.TopCategoryUrl = (product.Categories != null && product.Categories.Count > 0) ? product.Categories.ToList()[0].Url : "giyim";
            return productListModel;
        }
        public async Task<bool> UpdateFilterBoxName(string filterBoxId, string filterBoxName)
        {
            FilterBox filterBox = await _filterBoxRepository.GetByIdAsync(filterBoxId);
            filterBox.FilterBoxTitle = filterBoxName;
            _filterBoxRepository.Update(filterBox);
            await _filterBoxRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteFilterBox(string filterBoxId)
        {
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Filters).FirstOrDefaultAsync(fb => fb.Id.ToString() == filterBoxId);
            _filterRepository.RemoveRange(filterBox.Filters.ToList());
            await _filterRepository.SaveAsync();
            _filterBoxRepository.Remove(filterBox);
            await _filterBoxRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateFilterBox(string filterBoxId, string filterBoxName, string[] categoryIds)
        {
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Categories).FirstOrDefaultAsync(fb => fb.Id.ToString() == filterBoxId);
            List<Category> categories = new();
            for (var i = 0; i < categoryIds.Count(); i++)
            {
                Category category = await _categoryRepository.GetByIdAsync(categoryIds[i].ToString());
                categories.Add(category);
            }
            filterBox.Categories = categories;
            filterBox.FilterBoxTitle = filterBoxName;
            _filterBoxRepository.Update(filterBox);
            await _filterBoxRepository.SaveAsync();

            return true;
        }
    }
}
