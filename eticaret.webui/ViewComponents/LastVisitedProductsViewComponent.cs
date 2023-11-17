using eticaret.business.Abstract.Service;
using eticaret.entity.Category;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Product;
using Microsoft.AspNetCore.Mvc;

namespace eticaret.webui.ViewComponents
{
    public class LastVisitedProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public LastVisitedProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    
    }
}
