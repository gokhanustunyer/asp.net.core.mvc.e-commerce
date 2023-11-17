using eticaret.business.Abstract.Service;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Product;
using Microsoft.AspNetCore.Mvc;

namespace eticaret.webui.ViewComponents
{
    public class MaintenanceViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    
    }
}
