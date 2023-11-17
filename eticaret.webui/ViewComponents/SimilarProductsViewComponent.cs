using eticaret.business.Abstract.Service;
using eticaret.entity.Category;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Product;
using Microsoft.AspNetCore.Mvc;

namespace eticaret.webui.ViewComponents
{
    public class SimilarProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public SimilarProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryId, string productId)
        {
            List<ProductListModel> model = await _productService.GetProductsBasedOnSimilarFiltersByCategory(categoryId, productId);
            return View(model);
        }
    
    }
}
