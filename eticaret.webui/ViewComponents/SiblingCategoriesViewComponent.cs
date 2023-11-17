using eticaret.business.Abstract.Service;
using eticaret.entity.EntityRefrences.CategoryReference;
using Microsoft.AspNetCore.Mvc;

namespace eticaret.webui.ViewComponents
{
    public class SiblingCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public SiblingCategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryId)
        {
            List<CategoryListModel> model = await _categoryService.GetSiblingCategoriesAsync(categoryId);
            return View(model);
        }
    }
}
