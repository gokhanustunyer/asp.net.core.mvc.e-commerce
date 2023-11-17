using Microsoft.AspNetCore.Mvc;
using eticaret.business.Abstract.Service;
using eticaret.entity.EntityRefrences.ProductCategoryReference;
using eticaret.entity.PageEntities;
using eticaret.entity.EntityRefrences.ProductReference;

namespace eticaret.webui.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IGeneralService _generalService;

        public HomeController(IProductService productService,
                              IGeneralService generalService)
        {
            _productService = productService;
            _generalService = generalService;
        }
        [HttpGet] public IActionResult Index()
        {
            return View();
        }
        [HttpPost] public async Task<IActionResult> Index(SupportFormModel model)
        {
            await _generalService.SendSupportMail(model);
            return Redirect("/");
        }
        [HttpGet] public async Task<IActionResult> Products(string? category, string filters, string order)
        {
            ViewData["Query"] = filters;
            ProductCategoryPageModel model = await _productService.GetProductsByFilters(category, order, filters);
            if (model == null)
            {
                return Redirect("/NotFounded");
            }
            ViewData["Category"] = (category == null) ? "Giyim" : category;
            return View(model);
        }
        [HttpGet] public async Task<IActionResult> ProductDetails(string url)
        {
            var model = await _productService.GetProductDetails(url);
            if (model.Name == null)
                return Redirect("/NotFounded");
            return View(model);
        }
        [HttpGet] public IActionResult NotFounded()
        {
            return View();
        }
        [HttpGet] [Route("sitemap.xml")]  public IActionResult Sitemap()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetProductsById(string prIds)
        {
            IEnumerable<string> productIds = prIds.Split(',').Take(8);
            List<ProductListModel> model = await _productService.GetProductsByIds(productIds);
            return Ok(model);
        }
    }
}