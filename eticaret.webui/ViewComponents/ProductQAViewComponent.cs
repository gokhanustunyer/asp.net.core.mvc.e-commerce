using eticaret.business.Abstract.Service;
using Microsoft.AspNetCore.Mvc;

namespace eticaret.webui.ViewComponents
{
    public class ProductQAViewComponent: ViewComponent
    {
        private readonly IProductService _productService;
        public ProductQAViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string productId)
        {
            var model = await _productService.GetQAs(productId);
            var publisher = await _productService.GetProductPublisherById(productId);
            ViewData["publisher"] = publisher;
            ViewData["productId"] = productId;
            return View(model);
        }

    }
}
