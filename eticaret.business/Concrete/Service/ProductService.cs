using AutoMapper;
using eticaret.business.Abstract.Service;
using eticaret.business.Abstract.Storage.Azure;
using eticaret.business.Abstract.Storage.Local;
using eticaret.data.Abstract.Category;
using eticaret.data.Abstract.Order;
using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using eticaret.entity.Category;
using eticaret.entity.EntityRefrences.CategoryReference;
using eticaret.entity.EntityRefrences.ProductCategoryReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Filter;
using eticaret.entity.Identity;
using eticaret.entity.Offer;
using eticaret.entity.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ILocalStorage _localStorage;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly IFilterService _filterService;
        private readonly ISizeService _sizeService;
        private readonly ETicaretDbContext _context;
        private readonly UserManager<AppUser> _userManaer;


        private readonly string activeStoragePath = "";
        public ProductService(IProductRepository productRepository,
                              IConfiguration configuration,
                              ICategoryRepository categoryRepository,
                              IProductImageRepository productImageRepository,
                              ILocalStorage localStorage,
                              IMapper mapper,
                              ICategoryService categoryService,
                              IFilterService filterService,
                              ISizeService sizeService,
                              ETicaretDbContext context,
                              UserManager<AppUser> userManaer)
        {
            _productRepository = productRepository;
            _configuration = configuration;
            _categoryRepository = categoryRepository;
            _productImageRepository = productImageRepository;
            _localStorage = localStorage;
            _mapper = mapper;
            _categoryService = categoryService;
            _filterService = filterService;
            _sizeService = sizeService;
            _context = context;
            _userManaer = userManaer;

            string acStorageName = _configuration["ActiveStorage"];
            activeStoragePath = _configuration[$"StoragePaths:{acStorageName}"];
        }
        public async Task<bool> CreateAsync(ProductCreateModel model, Guid[] categoryIds)
        {
            List<Category> categories = new ();
            for (var i=0;i<categoryIds.Count();i++){
                categories.Add(await _categoryRepository.
                    GetByIdAsync(categoryIds[i].ToString())); }
            Product product = new() { Name = model.Name, Id = Guid.NewGuid(), CreateDate = DateTime.Now, UpdateDate = DateTime.Now, Price = model.Price, Stock = model.Stock, Description = model.Description, Categories = categories };
            await _productRepository.AddAsync(product);
            await _productRepository.SaveAsync();
            return true;
        }
        public async Task<bool> EditAsync(string id, ProductEditModel p, Guid[] categoryIds)
        {
            Product product = await _productRepository.Table
                    .Include(d => d.Categories).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            List<Category> categories = new() { };
            for (var i = 0; i < categoryIds.Count(); i++)
            {
                Category category = await _categoryRepository.GetByIdAsync
                        (categoryIds[i].ToString());
                categories.Add(category);
            }
            product.Categories = categories;
            product.UpdateDate = DateTime.Now;
            product.Name = p.Name;
            product.Stock = p.Stock;
            product.Price = p.Price;
            product.Description = p.Description;
            _productRepository.Update(product);
            await _productRepository.SaveAsync();

            return true;
        }
        public async Task<ProductEditModel> GetWithAllProperties(string id)
        {
            Product? product = await _productRepository.Table
                                                       .Include(p => p.ProductImages)
                                                       .Include(p => p.Categories)
                                                       .Include(p => p.RelatedProducts)
                                                       .Include(p => p.Filters)
                                                       .Include(p => p.ProductSizes)
                                                       .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            ProductEditModel productEditModel = _mapper.Map<ProductEditModel>(product);
            ProductImage firstProductImage = product.ProductImages.First();
            string storagePath = activeStoragePath;
            if (firstProductImage != null)
            {
                storagePath = _configuration[$"StoragePaths:{firstProductImage.Storage}"];
            }
            productEditModel.Path = storagePath;
            productEditModel.Price = Math.Round(productEditModel.Price, 2);
            productEditModel.ProductImages = product.ProductImages.ToList();
            productEditModel.ActiveCategories = product.Categories.ToList();
            productEditModel.Options = product.ProductSizes.ToList();
            productEditModel.AllCategories = _categoryRepository.GetAll().ToList();
            productEditModel.Url = product.Url;
            List<RelatedProduct> relateds = _productRepository.Context.RelatedProducts.Where
                                                                    (p => p.ProductId == product.Id.ToString() ||
                                                                    p.RelatedProductId == product.Id.ToString()).ToList();
            productEditModel.RelatedIds = new();
            foreach(RelatedProduct related in relateds)
            {
                productEditModel.RelatedIds.Add(related.ProductId);
                productEditModel.RelatedIds.Add(related.RelatedProductId);
            }

            productEditModel.Filters = product.Filters.ToList();
            return productEditModel;
        }
        public async Task<bool> AddImage(IFormFileCollection postedFiles, string id)
        {
            var datas = await _localStorage.UploadAsync("product-images", postedFiles);
            Product product = await _productRepository.GetByIdAsync(id);
            await _productImageRepository.AddRangeAsync
                (
                    datas.Select(d => new ProductImage()
                    {
                        FileName = d.fileName,
                        Path = d.path,
                        Storage = "Azure",
                        Product = new List<Product>() { product }
                    }).ToList()
                );
            await _productImageRepository.SaveAsync();
            return true;
        }
        public async Task<bool> RemoveAsync(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteImage(string id)
        {
            await _productImageRepository.RemoveAsync(id);
            await _productImageRepository.SaveAsync();
            return true;
        }
        public async Task<List<ProductListModel>> GetAll()
        {
            List<Product> products = _productRepository.GetAll().ToList();
            List<ProductListModel> model = new() { };
            for (var i = 0; i < products.Count(); i++)
            {
                model.Add(await ProductToListModel(products[i]));
            }
            return model;
        }
        public async Task<ProductCategoryPageModel> GetLastProducts(string categoryName, string order="new")
        {
            if (categoryName == null)
            {
                categoryName = "Giyim";
            }

            Category? category = await _categoryRepository.Table.FirstOrDefaultAsync(c => c.Url == categoryName);
            List<Category> categories = await _categoryService.GetSubCategories(category);
            List<CategoryListModel> categoryListModels = new();
            List<string> categoryIds = new();
            for (var j = 0; j < categories.Count(); j++)
            {
                categoryIds.Add(categories[j].Id.ToString());
                categoryListModels.Add(new CategoryListModel()
                {
                    Id = categories[j].Id,
                    Name = categories[j].Name,
                    Url = categories[j].Url
                });
            }

            List<Product> products = _productRepository.Table.Include(p => p.Rates).Include(p => p.Campaigns).Include(d => d.ProductImages).Where(p => p.Categories.Contains(category)).ToList();
            if (order == "new" || order == null)
            {
                products = products.OrderByDescending(p => p.CreateDate).ToList();
            }
            else if (order == "grow")
            {
                products = products.OrderBy(p => p.Price).ToList();
            }
            else
            {
                products = products.OrderByDescending(p => p.Price).ToList();
            }

            List<ProductListModel> productListModels = new();
            for(var i = 0; i < products.Count(); i++)
            {
                var productListModel = await ProductToListModel(products[i]);
                if (products[i].ProductImages.Count > 0)
                {
                    productListModel.ImageUrl
                        = activeStoragePath
                        + $"{products[i].ProductImages.ToList()[0].Path}";
                }
                productListModels.Add(productListModel);
            }

            List<FilterBox> filters = await _filterService.GetFilterByCategoryAsync(new() { category.Id.ToString() });
            ProductCategoryPageModel model = new()
            {
                Products = productListModels,
                Categories = categoryListModels,
                Filters = filters,
                FilterIds = new List<string>(),
                CategoryPath = await _categoryService.GetByIdWithFullDepth(category.Id.ToString())
            };
            return model;
        }
        public async Task<ProductCategoryPageModel> GetProductsByFilters(string categoryName, string order="new", string? filterQuery = "")
        {
            List<string> filterIds = new() { };
            List<Product> products = new() { };
            List<string> categoryPath = new();

            if (categoryName == null)
            {
                categoryName = "Giyim";
            }
            Category? category = await _categoryRepository.Table.FirstOrDefaultAsync(c => c.Url == categoryName);
            List<Category> categories = await _categoryService.GetSubCategories(category);
            List<CategoryListModel> categoryListModels = new();
            List<string> categoryIds = new();
            for (var j = 0; j < categories.Count(); j++)
            {
                categoryIds.Add(categories[j].Id.ToString());
                CategoryImage categoryImage = categories[j].CategoryImage;
                string? imagePath;
                if (categoryImage != null)
                {
                    imagePath = activeStoragePath + categoryImage.Path;
                }
                else
                {
                    imagePath = null;
                }
                categoryListModels.Add(new CategoryListModel()
                { 
                    Id = categories[j].Id,
                    Name = categories[j].Name,
                    Url = categories[j].Url,
                    ImagePath = imagePath
                });
            }
            
            if (filterQuery != null)
            {
                filterIds = filterQuery.Split("|").ToList();
                filterIds.RemoveAt(filterIds.Count() - 1);
                products = _productRepository.Table.Include(p => p.Campaigns).Include(d => d.ProductImages).Include(p => p.Filters).Where(p => p.Filters.Where(f => filterIds.Contains(f.Id.ToString())).ToList().Count > 0).Where(p => p.Categories.Contains(category)).ToList();
            }
            else
            {
                products = _productRepository.Table.Include(p => p.Campaigns).Include(d => d.ProductImages).Where(p => p.Categories.Contains(category)).ToList();
            }
            if (order == "new" || order == null)
            {
                products = products.OrderByDescending(p => p.CreateDate).ToList();
            }
            else if (order == "grow")
            {
                products = products.OrderBy(p => p.Price).ToList();
            }
            else
            {
                products = products.OrderByDescending(p => p.Price).ToList();
            }

            List<ProductListModel> productListModels = new();
            for (var i = 0; i < products.Count(); i++)
            {
                var productListModel = await ProductToListModel(products[i]);
                if (products[i].ProductImages.Count > 0)
                {
                    productListModel.ImageUrl
                        = activeStoragePath
                        + $"{products[i].ProductImages.ToList()[0].Path}";
                }
                productListModels.Add(productListModel);
            }
            List<FilterBox> filters = await _filterService.GetFilterByCategoryIdAsync(category.Id.ToString());
            ProductCategoryPageModel model = new()
            {
                Products = productListModels,
                Categories = categoryListModels,
                Filters = filters,
                FilterIds = filterIds,
                CategoryPath = await _categoryService.GetByIdWithFullDepth(category.Id.ToString())
            };
            return model;

        }
        public async Task<ProductDetailsModel> GetProductDetails(string url)
        {
            Product? product = await _productRepository.Table
                                                       .Include(p => p.Campaigns)
                                                       .Include(p => p.Rates)
                                                       .Include(d => d.Categories)
                                                       .Include(p => p.RelatedProducts)
                                                       .Include(d => d.ProductImages)
                                                       .Include(p => p.ProductSizes)
                                                       .ThenInclude(p => p.Size)
                                                       .FirstOrDefaultAsync(p => p.Url == url);
            if (product == null)
            {
                return new();
            }
            product.ProductImages = product.ProductImages.OrderBy(pi => pi.Index).ToList();
            List<string> imagePaths = new() { };
            foreach(ProductImage productImage in product.ProductImages)
            {
                string storagePath = _configuration[$"StoragePaths:{productImage.Storage}"];
                imagePaths.Add(storagePath + $"{productImage.Path}");
            }
            int sum = 0;
            if (product.Rates != null || product.Rates?.Count > 0)
            {
                sum = product.Rates.Select(r => r.Rate).Sum();
            }
            if (sum > 0)
            {
                sum /= product.Rates.Count;
            }
            List<ProductListModel> relateds = new();
            List<RelatedProduct>  relatedIds = _productRepository.Context
                                               .RelatedProducts.Where
                                               (p => p.ProductId == product.Id.ToString() || 
                                         p.RelatedProductId == product.Id.ToString()).ToList();
            for (int i = 0; i < relatedIds.Count; i++)
            {
                if (relatedIds[i].ProductId != product.Id.ToString())
                {
                    relateds.Add(await ProductToListModel(
                            await _productRepository.GetByIdAsync(relatedIds[i].ProductId)));
                }
                else
                {
                    relateds.Add(await ProductToListModel(
                            await _productRepository.GetByIdAsync(relatedIds[i].RelatedProductId)));
                }
            }

            var mainImage = product.ProductImages.ToList()[0];
            List<OptionModel> options = new();
            List<string> sortedSizesNames = new() { "XXS", "XS", "S", "M", "L", "XL", "XXL", "XXXL", "3XL", "4XL", "5XL", "6XL", "7XL", "8XL", "9XL", "10XL" };
            List<ProductSize> productSizes = product.ProductSizes.ToList();
            List<OptionModel> extracts = new();
            for (var i = 0; i < sortedSizesNames.Count; i++)
            {
                if (options.Count + extracts.Count == productSizes.Count) { break; }
                for (var j = 0; j < productSizes.Count; j++)
                {
                    if (sortedSizesNames[i] == productSizes[j].SizeName)
                    {
                        options.Add(new() { Size = productSizes[j].Size, Name = sortedSizesNames[i], Count = productSizes[j].Stock, Price = Math.Round(productSizes[j].Price,2)});
                    }
                    else
                    {
                        var ext = new OptionModel() 
                        { 
                            Count = productSizes[j].Stock,
                            Name = productSizes[j].SizeName,
                            Price = Math.Round(productSizes[j].Price,2),
                            Size = productSizes[j].Size
                        };
                        if (!options.Contains(ext) && !sortedSizesNames.Contains(ext.Name))
                        {
                            extracts.Add(ext);
                        }
                    }
                }
            }
            options = options.Concat(extracts).ToList();
            ProductDetailsModel model = new()
            {
                Name = product.Name,
                Description = product.Description,
                Id = product.Id,
                Price = Math.Round(product.Price,2),
                Categories = product.Categories.OrderBy(c => c.UpdateDate).ToList(),
                ShortDescription = product.ShortDescription,
                ImagePaths = imagePaths,
                MainImagePath = $"{_configuration[$"StoragePaths:{mainImage.Storage}"]}{mainImage.Path}",
                RateCount = product.Rates.Count,
                RelatedProducts = relateds,
                Options = options,
                Rate = sum,
                QuestionedCategoryUrl = product.Categories.ToList()[0].Url
            };
            if (product.Campaigns != null)
            {
                int maxOffer = 0;
                for (int i = 0; i < product.Campaigns.Count; i++)
                {
                    if (product.Campaigns[i].CodeEndDate < DateTime.Now)
                    {
                        Campaign removeCampaign = await _context.Campaigns.Include(c => c.Categories).Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == product.Campaigns[i].Id);
                        _context.Campaigns.Remove(removeCampaign);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (product.Campaigns[i].DiscountRate > maxOffer)
                        {
                            maxOffer = product.Campaigns[i].DiscountRate;
                        }
                    }
                }
                model.DiscountRate = maxOffer;
            }
            return model;
        }
        public async Task<ProductListModel> GetByIdWithImage(string id)
        {
            Product product = await _productRepository.Table.Include(d => d.ProductImages).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            if (product != null)
            {
                return await ProductToListModel(product);
            }
            else
            {
                return null;
            }
        }
        public async Task<List<ProductListModel>> GetAllWithImages()
        {
            List<Product> products = _productRepository.Table.OrderByDescending(p => p.CreateDate).ToList();
            List<ProductListModel> productListModels = new();
            for (var i = 0; i < products.Count(); i++) 
            {
                productListModels.Add(await ProductToListModel(products[i]));
            }
            return productListModels;
        }
        public async Task<List<ProductCommentModel>> GetCommentsAsync(string productId, bool haveImage)
        {
            var product = await _productRepository.Table
                                                  .Include(p => p.ProductImages)
                                                  .Include(p => p.Comments)
                                                  .ThenInclude(c => c.Rate)
                                                  .ThenInclude(r => r.User)
                                                  .Include(p => p.Comments)
                                                  .ThenInclude(c => c.Images)
                                                  .FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));
            List<ProductCommentModel> model = new();
            List<ProductComment> comments = product.Comments.Where(p => p.isHavePhoto == haveImage).ToList();
            comments = comments.OrderByDescending(x => x.UpdateDate).ToList();
            int sum = 0;
            if (product.Rates != null || product.Rates?.Count > 0)
            {
                sum = product.Rates.Select(r => r.Rate).Sum();
            }
            if (sum > 0)
            {
                sum /= product.Rates.Count;
            }
            for (int i = 0; i < comments.Count; i++)
            {
                ProductCommentModel comment = new()
                {
                    Comment = comments[i].Comment,
                    FullName = $"{comments[i].Rate.User.FirstName} {comments[i].Rate.User.LastName}",
                    Rate = comments[i].Rate.Rate,
                    UpdateDate = $"{comments[i].UpdateDate:dd/MM/20yy}",
                    Product = await ProductToListModel(product),
                    GeneralRate = sum
                };
                if (haveImage)
                {
                    List<string> imagePaths = new();
                    foreach(CommentImage image in comments[i].Images)
                    {
                        imagePaths.Add($"{activeStoragePath}{image.Path}");
                    }
                    comment.ImagePaths = imagePaths;
                }
                model.Add(comment);
            }
            return model;
        }
        public async Task<List<ProductListModel>> Search(string searchWords)
        {
            List<string> splitterWords = searchWords.Split(" ").ToList();
            List<ProductListModel> search_result = new();
            for (int i = 0; i < splitterWords.Count; i++)
            {
                if (splitterWords[i] == "" || splitterWords[i] == " ") { continue; }
                List<Product> products = _productRepository.Table
                                            .Where(p => p.Name.Contains(splitterWords[i]) ||
                                            p.Url.Contains(splitterWords[i])).ToList();
                List<ProductListModel> active_result = new();
                for(int j = 0; j < products.Count; j++) { active_result.Add(await ProductToListModel(products[j])); }
                search_result = search_result.Concat(active_result.ToList()).ToList();
                search_result = search_result.Distinct().ToList();
            }
            return search_result;
        }
        public async Task<bool> DeleteSizeOnProduct(string sizeId, string productId)
        {
            Product product = await _productRepository.Table.Include(p => p.ProductSizes).FirstOrDefaultAsync(p => p.Id.ToString() == productId);
            ProductSize size = product.ProductSizes.FirstOrDefault(p => p.SizeId.ToString() == sizeId);
            product.ProductSizes.Remove(size);
            _productRepository.Update(product);
            await _productRepository.SaveAsync();
            return true;
        }
        public async Task<List<ProductSize>> GetSizesById(string productId)
        {
            Product product = await _productRepository.Table.Include(p => p.ProductSizes).FirstOrDefaultAsync(p => p.Id.ToString() == productId);
            foreach(ProductSize pr in product.ProductSizes) { pr.Product = null;pr.Price = Math.Round(pr.Price, 2); };
            return product?.ProductSizes.ToList();
        }
        public async Task<bool> AddNewOption(string ProductId, string SizeName, double Price, int Stock)
        {
            Product product = await _productRepository.Table.Include(p => p.ProductSizes).FirstOrDefaultAsync(p => p.Id.ToString() == ProductId);
            Size size = _sizeService.GetByName(SizeName);
            if (size == null)
                size = await _sizeService.GenerateSize(SizeName);
            product.ProductSizes.Add(new ProductSize()
            {
                Price = Price,
                Product = product,
                ProductId = Guid.Parse(ProductId),
                SizeName = SizeName,
                Stock = Stock,
                SizeId = size.Id
            });
            _productRepository.Update(product);
            await _productRepository.SaveAsync();
            return true;
        }
        public async Task<List<ProductListModel>> ProductToListModelRange(List<Product> products)
        {
            var result = new List<ProductListModel>();
            foreach(Product product in products)
            {
                result.Add(await ProductToListModel(product));
            }
            return result;
        }
        public async Task<ProductListModel> ProductToListModel(Product product)
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
            if (product.Campaigns != null && product.Campaigns.Count > 0)
            {
                int maxOffer = 0;
                for (int i = 0; i < product.Campaigns.Count; i++)
                {
                    if (product.Campaigns[i].CodeEndDate < DateTime.Now)
                    {
                        
                        Campaign removeCampaign = await _context.Campaigns.Include(c => c.Categories).Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == product.Campaigns[i].Id);
                        _context.Campaigns.Remove(removeCampaign);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (product.Campaigns[i].DiscountRate > maxOffer)
                        {
                            maxOffer = product.Campaigns[i].DiscountRate;
                        }
                    }
                }
                productListModel.DiscountedPrice = Math.Round(product.Price * (100 - maxOffer) / 100,2);
            }
            if (product.ProductImages == null)
            {
                product.ProductImages = (await _productRepository.Table.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == product.Id)).ProductImages;
            }
            if (product.ProductImages != null && product.ProductImages.Count > 0)
            {
                product.ProductImages = product.ProductImages.OrderBy(pi => pi.Index).ToList();
                string mainImageId = product.ProductImages.First().Id.ToString();
                var mainImage = await _productImageRepository.GetByIdAsync(mainImageId);
                productListModel.MainImageUrl = product.ProductImages.First() != null ? $"{_configuration[$"StoragePaths:{mainImage.Storage}"]}{mainImage.Path}" : null;
            }
            else
            {
                string mainImageId = product.MainImageId.ToString();
                var mainImage = await _productImageRepository.GetByIdAsync(mainImageId);
                productListModel.MainImageUrl = $"{_configuration[$"StoragePaths:{mainImage.Storage}"]}{mainImage.Path}";
            }
            if (product.Rates != null)
            {
                if (product.Rates.Count() > 0)
                {
                    productListModel.Rate = (int?)Math.Ceiling((decimal)(product.Rates.ToList().Sum(r => r.Rate) / product.Rates.Count()));
                }
                else
                {
                    productListModel.Rate = null;
                }
            }
            //productListModel.ImageUrl = product.ProductImages?.Count > 0 ? $"{_configuration["StoragePaths:Azure"]}" + $"{product.ProductImages.ToList()[0].Path}" : null;
            productListModel.TopCategoryUrl = (product.Categories != null && product.Categories.Count > 0) ? product.Categories.ToList()[0].Url : "giyim";
            return productListModel;
        }
        public async Task<List<ProductListModel>> GetMostSellings(int productCount)
        {
            List<Tuple<string, int>> result = _context.OrderItems.Select(oi => Tuple.Create(oi.ProductId, oi.Quantity)).ToList();
            Dictionary<string, int> uniqueDict = new();
            foreach(Tuple<string, int> item in result)
            {
                var pr = await _productRepository.GetByIdAsync(item.Item1);
                if (pr != null)
                {

                    if (uniqueDict.ContainsKey(item.Item1))
                    {
                        uniqueDict[item.Item1] += item.Item2;
                    }
                    else
                    {
                        uniqueDict.Add(item.Item1, item.Item2);
                    }
                }
            }

            var sortedDict = from entry in uniqueDict orderby entry.Value ascending select entry;
            List<string> productIds = sortedDict.Select(d => d.Key).ToList();
            productIds.Reverse();
            productIds = productIds.Take(productCount).ToList();
            List<ProductListModel> productListModels = new();
            foreach (string id in productIds) { productListModels.Add(await GetByIdWithImage(id)); }

            return productListModels;
        }
        public async Task<List<ProductListModel>> GetProductsByCategoryIds(string CategoryIds)
        {
            List<string> categoryIds = CategoryIds.Split(",").ToList();
            List<Product> products = _productRepository.Table.Include(p => p.Categories).Where(p => p.Categories.FirstOrDefault(c => categoryIds.Contains(c.Id.ToString())) != null).ToList();
            List<ProductListModel> productListModels =  await ProductToListModelRange(products);
            return productListModels;
        }
        public async Task<List<ProductListModel>> GetProductsByCategoryIds(string CategoryIds, int count)
        {
            List<string> categoryIds = CategoryIds.Split(",").ToList();
            List<Product> products = _productRepository.Table.Include(p => p.Categories).Where(p => p.Categories.FirstOrDefault(c => categoryIds.Contains(c.Id.ToString())) != null).OrderByDescending(p => p.CreateDate).Take(count).ToList();
            List<ProductListModel> productListModels = await ProductToListModelRange(products);
            return productListModels;
        }
        public async Task<List<ProductQAListModel>> GetQAs(string id)
        {
            List<ProductQAListModel> result = new();
            Product product = await _productRepository.Table.Include(p => p.QAs).ThenInclude(qa => qa.User).FirstOrDefaultAsync(p => p.Id.ToString() == id);
            List<ProductQA> productQAs = product.QAs.Where(qa => qa.UpperQA == null).ToList();
            foreach (ProductQA productQA in productQAs)
            {
                List<ProductQAListModel> responses = _context.ProductQAs.Include(qa => qa.UpperQA).Where(qa => qa.UpperQA.Id == productQA.Id).OrderBy(qa => qa.CreateDate).Select(qa => new ProductQAListModel()
                {
                    FullName = $"{qa.User.FirstName} {qa.User.LastName}",
                    Message = qa.Message,
                    PublishDate = qa.CreateDate,
                    Id = qa.Id.ToString()
                }).ToList();
                result.Add(new ProductQAListModel()
                {
                    FullName = $"{productQA.User.FirstName} {productQA.User.LastName}",
                    Message = productQA.Message,
                    PublishDate = productQA.CreateDate,
                    Responses = responses,
                    Id = productQA.Id.ToString()
                });
            }
            return result;
        }
        public async Task<Product> CreateQa(ProductQaCreateModel model)
        {
            Product product = await _productRepository.Table.Include(p => p.QAs).FirstOrDefaultAsync(p => p.Id.ToString() == model.ProductId);
            AppUser user = await _userManaer.FindByNameAsync(model.UserName);
            ProductQA productQA = new()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                Message = model.QA,
                Product = product,
                User = user
            };
            _context.ProductQAs.Add(productQA);
            await _productRepository.SaveAsync();
            return product;
        }
        public async Task<AppUser> GetProductPublisherById(string productId)
            => (await _productRepository.Table.Include(p => p.Publisher).FirstOrDefaultAsync(p => p.Id.ToString() == productId)).Publisher;
        public async Task<Product> SendQaResponse(string message, string productId, string qaId, string username)
        {
            Product product = await _productRepository.Table.Include(p => p.QAs).ThenInclude(q => q.UpperQA).Include(p => p.Publisher).FirstOrDefaultAsync(p => p.Id.ToString() == productId);
            AppUser user = await _userManaer.FindByNameAsync(username);
            bool isAdmin = await _userManaer.IsInRoleAsync(user, "Admin");
            if (username == product.Publisher.UserName || isAdmin)
            {
                ProductQA selectedQA = product.QAs.FirstOrDefault(qa => qa.Id.ToString() == qaId);
                ProductQA newProductQA = new ()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Message = message,
                    Product = product,
                    User = user,
                    UpperQA = selectedQA
                };
                _context.ProductQAs.Add(newProductQA);
                await _productRepository.SaveAsync();
            }
            return product;
        }

        public async Task<List<ProductListModel>> GetProductsByIds(IEnumerable<string> productIds)
        {
            List<Product> products = new();
            foreach(string productId in productIds)
            {
                var target = await _productRepository.Table.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));
                if (target != null)
                {
                    products.Add(target);
                }
            }
            List<ProductListModel> productListModels = await ProductToListModelRange(products);
            productListModels.Reverse();
            return productListModels;
        }

        public async Task<List<ProductListModel>> GetProductsBasedOnSimilarFiltersByCategory(string categoryId, string productId)
        {
            // Defining our variables
            List<Product> filteredProducts = new();
            // Getting target category from args.
            Category targetCategory = await _categoryRepository.GetByIdAsync(categoryId);
            // Getting target product from args.
            Product targetProduct = await _productRepository.Table.Include(p => p.Filters).ThenInclude(f => f.Products).ThenInclude(p => p.Categories).FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));
            // Throw an argument null exception of product or category is null.
            if (targetProduct == null || targetCategory == null)
            {
                throw new ArgumentNullException();
            }
            // Get products who has our target category.
            IEnumerable<Product> products = _productRepository.Table.Include(p => p.Categories).Where(p => p.Categories.Contains(targetCategory));
            // Our product have to have one or more filter.
            if (targetProduct.Filters != null)
            {
                foreach (Filter filter in targetProduct.Filters)
                {
                    if (filter.Products != null)
                    {
                        List<Product> filteredByCategory = filter.Products.Where(f => f.Categories.FirstOrDefault(c => c.Id == targetCategory.Id) != null).ToList();
                        filteredProducts.AddRange(filteredByCategory.Where(f => !filteredProducts.Contains(f) && f.Id != Guid.Parse(productId)));
                    }
                }
            }

            return await ProductToListModelRange(filteredProducts);
        }
    }
}
