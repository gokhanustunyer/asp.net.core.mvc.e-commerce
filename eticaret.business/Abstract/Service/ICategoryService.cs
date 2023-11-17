using eticaret.entity.Category;
using eticaret.entity.EntityRefrences.CategoryReference;
using eticaret.entity.EntityRefrences.ProductReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category GetById(string id);
        bool CreateAsync(Category model);
        bool UpdateAsync(Category model);
        bool DeleteAsync(Category model);
        List<CategoryListModel> GetAllByListModel();
        List<CategoryListModel> GetAllWithAllProperties();
        Task<CategoryEditModel> GetByIdForEdit(string id);
        Task<bool> ClearCategoriesFromCategory(string id);
        Task<List<Category>> GetTopCategories(Category category);
        Task<List<Category>> GetSubCategories(Category category, int deep=-1);
        List<TopCategory> GetAllWithFullDeep();
        Task<List<Category>> GetByIdWithFullDepth(string id);
        Task<CategoryLinkedModel> CategoryToLinkedModel(Category head);
        Task<List<CategoryListModel>> GetSiblingCategoriesAsync(string categoryId);
    }
}
