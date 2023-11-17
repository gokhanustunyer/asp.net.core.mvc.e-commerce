using AutoMapper;
using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Category;
using eticaret.entity.Category;
using eticaret.entity.EntityRefrences.CategoryReference;
using eticaret.entity.EntityRefrences.ProductReference;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository,
                               IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public bool CreateAsync(Category model)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAsync(Category model)
        {
            throw new NotImplementedException();
        }


        public bool UpdateAsync(Category model)
        {
            throw new NotImplementedException();
        }
        public List<Category> GetAll()
            => _categoryRepository.GetAll().ToList();
            

        public Category GetById(string id)
        {
            throw new NotImplementedException();
        }


        public List<CategoryListModel> GetAllByListModel()
        {
            List<Category> categories = GetAll();
            List<CategoryListModel> categoryListModels = new();
            for (var i = 0; i < categories.Count; i++) 
                { categoryListModels.Add(CategoryToListModel(categories[i])); }
            return categoryListModels;
        }

        public List<CategoryListModel> GetAllWithAllProperties()
        {
            List<Category> categories = _categoryRepository.Table.ToList();
            List<CategoryListModel> categoryListModels = new();
            for (var i = 0; i < categories.Count; i++)
                { categoryListModels.Add(_mapper.Map<CategoryListModel>(categories[i])); }
            return categoryListModels;
        }

        public async Task<CategoryEditModel> GetByIdForEdit(string id)
        {
            Category? category = await _categoryRepository.Table
                                                          .Include(c => c.CategoryImage)
                                                          .FirstOrDefaultAsync(c => c.Id == Guid.Parse(id));
            CategoryEditModel categoryEditModel = _mapper.Map<CategoryEditModel>(category);

            if (category.CategoryImage != null)
            {
                string? imagePath = null;
                imagePath = category.CategoryImage.Path;
                categoryEditModel.ImagePath = imagePath;
            }
            categoryEditModel.AllCategories = GetAllWithFullDeep();
            categoryEditModel.SubCategories = _categoryRepository.Table.Where(c => c.TopCategoryId == id).ToList();
            categoryEditModel.TopCategories = await GetTopCategories(category);
            categoryEditModel.TopCategoryId = category.TopCategoryId;
            return categoryEditModel;
        }

        public async Task<bool> ClearCategoriesFromCategory(string id)
        {

            return true;
        }

        public async Task<List<Category>> GetTopCategories(Category category)
        {

            List <Category> topCategories = new();
            Category tempCategory = category;
            while (tempCategory.TopCategoryId != null)
            {
                tempCategory = await _categoryRepository
                        .GetByIdAsync(tempCategory.TopCategoryId);
                topCategories.Add(tempCategory);
            }
            return topCategories;
        }

        public async Task<List<Category>> GetSubCategories(Category category, int deep = -1)
        {
            return _categoryRepository.Table.Include(c => c.CategoryImage)
                                            .Where(c => c.TopCategoryId == category.Id.ToString()).ToList();
        }

        public List<TopCategory> GetAllWithFullDeep()
        {
            List<Category> topCategories = _categoryRepository.Table
                    .Where(c => c.TopCategoryId == null).ToList();
            List<TopCategory> result = new() { };
            int totalCategoryCount = _categoryRepository.Table.Count();
            int counter = 0;

            for (var i = 0; i < topCategories.Count; i++)
            {
                List<TopCategory> subCategories = _categoryRepository.Table.Where
                    (c => c.TopCategoryId == topCategories[i].Id.ToString())
                    .Select(c => new TopCategory 
                        { Id = c.Id.ToString(), Name = c.Name } ).ToList();
                result.Add(new TopCategory() { Id = topCategories[i].Id.ToString(), Name = topCategories[i].Name, SubCategories = setSubCategories(topCategories[i].Id.ToString()) } );
                //subCategories.ForEach(c => c.SubCategories = setSubCategories(c.Id));
                //for (var j = 0; j < subCategories.Count; j++)
                //    result.Add(subCategories[j]);
            }

            return result;
        }

        private List<TopCategory> setSubCategories(string categoryId)
        {
            List<Category> subCategories = _categoryRepository.Table
                .Where(c => c.TopCategoryId == categoryId).ToList();

            if (subCategories.Count == 0 || subCategories == null)
                return null;

            List<TopCategory> result = new();
            for (var i = 0; i < subCategories.Count; i++)
            {
                result.Add(new() 
                    { Id = subCategories[i].Id.ToString(),
                      Name = subCategories[i].Name,
                      SubCategories = setSubCategories(subCategories[i].Id.ToString())});
            }

            return result;
        }

        public async Task<List<Category>> GetByIdWithFullDepth(string id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id);
            List<Category> result = new() { category };
            Category temp = category;
            while (temp.TopCategoryId != null)
            {
                temp = await _categoryRepository.GetByIdAsync(temp.TopCategoryId);
                result.Add(temp);
            }
            result.Reverse();
            return result;
        }

        public async Task<CategoryLinkedModel> CategoryToLinkedModel(Category tail)
        {
            CategoryLinkedModel linkedModel = new CategoryLinkedModel();
            
            
            return linkedModel;
        }

        public async Task<List<CategoryListModel>> GetSiblingCategoriesAsync(string categoryId)
        {
            Category targetCategory = await _categoryRepository.GetByIdAsync(categoryId);
            IEnumerable<Category> siblingCategories = _categoryRepository.Table
                                                                         .Include(c => c.CategoryImage)
                                                                         .Where(c => c.TopCategoryId == targetCategory.TopCategoryId);
            return CategoryToListModelRange(siblingCategories.ToList());
        }

        private CategoryListModel CategoryToListModel(Category model)
            => new() { Id = model.Id, Name = model.Name, Url = model.Url, ImagePath = model.CategoryImage?.Path };

        private List<CategoryListModel> CategoryToListModelRange(List<Category> categories)
        {
            List<CategoryListModel> result = new();
            foreach (Category category in categories)
            {
                result.Add(CategoryToListModel(category));
            }
            return result;
        }
    }
}
