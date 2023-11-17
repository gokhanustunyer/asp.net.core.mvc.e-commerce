using eticaret.business.Abstract.Service;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Product;
using Microsoft.AspNetCore.Mvc;

namespace eticaret.webui.ViewComponents
{
    public class ProductCommentsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductCommentsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string productId, bool isImage)
        {
            var model = await _productService.GetCommentsAsync(productId, isImage);
            ViewBag.HaveImage = isImage;
            return View(model);
        }
    
    }
}
