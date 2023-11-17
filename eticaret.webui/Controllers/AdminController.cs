using eticaret.business.Abstract.Service;
using eticaret.entity.EntityRefrences.ProductReference;
using Microsoft.AspNetCore.Mvc;
using eticaret.business.Features.Commands.Product.CreateProduct;
using eticaret.business.Features.Commands.Product.EditProduct;
using eticaret.business.Features.Commands.ProductImageFile.UploadProductImage;
using eticaret.business.Features.Commands.ProductImageFile.RemoveProductImage;
using eticaret.business.Features.Commands.Category.CreateCategory;
using eticaret.entity.EntityRefrences.CategoryReference;
using eticaret.business.Features.Commands.Category.EditCategory;
using eticaret.business.Features.Commands.Category.DeleteCategory;
using eticaret.business.Features.Commands.Product.DeleteProduct;
using eticaret.business.Features.Commands.ProductImageFile.SetAsMainProductImage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using eticaret.entity.Identity;
using eticaret.business.Features.Commands.Role.CreateRole;
using MediatR;
using mkiyafetleri.webui.SiteOperations;
using eticaret.business.Abstract.Operations;
using eticaret.entity.EntityRefrences.PageStrings;
using eticaret.data.Abstract.Product;
using eticaret.entity.Product;
using eticaret.business.Features.Commands.Filter.CreateFilter;
using Newtonsoft.Json;
using eticaret.entity.Filter;
using eticaret.business.Features.Commands.ProductImageFile.SaveAlignment;
using eticaret.business.Features.Commands.Product.UpdateSize;
using eticaret.business.Features.Commands.Cargo.UpdateShippingCompany;
using eticaret.entity.Cargo;
using eticaret.entity.PageEntities;
using eticaret.business.Features.Commands.Discount.CreateDiscountCode;
using eticaret.business.Features.Commands.Discount.EditDiscountCode;
using eticaret.business.Features.Commands.Discount.CreateCampaign;
using eticaret.business.Features.Commands.Discount.EditCampaign;

namespace eticaret.webui.Controllers
{
    [Authorize(Roles = "Admin")]
    //[AutoValidateAntiforgeryToken]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ISitemapService _sitemapService;
        private readonly IPageStringsService _pageStringService;
        private readonly IProductImageService _productImageService;
        private readonly IMediator _mediator;
        private readonly IGeneralService _generalService;
        private readonly IOrderService _orderService;
        private readonly IFilterService _filterService;
        private readonly IShippingService _shippingService;
        private readonly IDiscountService _discountService;
        public AdminController(IProductService productService,
                               ICategoryService categoryService,
                               IUserService userService,
                               RoleManager<AppRole> roleManager,
                               ISitemapService sitemapService,
                               IPageStringsService pageStringService,
                               IProductImageService productImageService,
                               IMediator mediator,
                               IGeneralService generalService,
                               IOrderService orderService,
                               IFilterService filterService,
                               IShippingService shippingService,
                               IDiscountService discountService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _userService = userService;
            _roleManager = roleManager;
            _sitemapService = sitemapService;
            _pageStringService = pageStringService;
            _productImageService = productImageService;
            _mediator = mediator;
            _generalService = generalService;
            _orderService = orderService;
            _filterService = filterService;
            _shippingService = shippingService;
            _discountService = discountService;
        }

        [HttpGet] public async Task<IActionResult> Index()
        {
            var model = await _generalService.GetStatistics(DateTime.Now.Year, DateTime.Now.Month);
            return View(model);
        }
        [HttpGet] public IActionResult AddProduct()
        {
            var categoryListModels = _categoryService.GetAllByListModel();
            var deepCategoryList = _categoryService.GetAllWithFullDeep();
            var model = new ProductCreateModel() 
                { Categories = categoryListModels, TopCategories = deepCategoryList };
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> AddProduct (CreateProductCommandRequest createProductCommandRequest)
        {
            if (!ModelState.IsValid)
            {
                return AddProduct();
            }
            TempData["Alert"] = JsonConvert.SerializeObject((await _mediator.Send(createProductCommandRequest)).Notice);
            return Redirect("/admin/addproduct");
        }
        [HttpGet] public IActionResult AddCategory()
        {
            var categoryListModels = _categoryService.GetAllByListModel();
            var deepCategoryList = _categoryService.GetAllWithFullDeep();
            var model = new CategoryCreateModel()
            { Categories = categoryListModels, TopCategories = deepCategoryList };
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> AddCategory (CreateCategoryCommandRequest createCategoryCommandRequest)
        {
            await _mediator.Send(createCategoryCommandRequest);
            return Redirect("/admin/addCategory");
        }
        [HttpGet]  public async Task<IActionResult> AdminProducts()
        {
            List<ProductListModel> products = await _productService.GetAllWithImages();
            return View(products);
        }
        [HttpGet] public IActionResult AdminCategories()
        {
            var deepCategoryList = _categoryService.GetAllWithFullDeep();
            return View(deepCategoryList);
        }
        [HttpGet] public async Task<IActionResult> EditCategory(string id)
        {
            CategoryEditModel model = await _categoryService.GetByIdForEdit(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditCategory (EditCategoryCommandRequest editCategoryCommandRequest)
        {
            await _mediator.Send(editCategoryCommandRequest);
            return Redirect("/admin/admincategories");
        }
        [HttpGet] public async Task<IActionResult> EditProduct(string id)
        {
            var deepCategoryList = _categoryService.GetAllWithFullDeep();
            ProductEditModel model = await _productService.GetWithAllProperties(id);
            model.TopCategories = deepCategoryList;
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditProduct (EditProductCommandRequest editProductCommandRequest)
        {
            await _mediator.Send(editProductCommandRequest);
            return Redirect($"/admin/editproduct/{editProductCommandRequest.id}");
        }
        [HttpPost] public async Task<IActionResult> DeleteCategory (DeleteCategoryCommandRequest deleteCategoryCommandRequest)
        {
            await _mediator.Send(deleteCategoryCommandRequest);
            return Redirect("/admin/admincategories");
        }
        [HttpPost] public async Task<IActionResult> DeleteProduct (DeleteProductCommandRequest deleteProductCommandRequest)
        {
            TempData["Alert"] = JsonConvert.SerializeObject((await _mediator.Send(deleteProductCommandRequest)).Notice);
            return Redirect("/admin/adminproducts");
        }
        [HttpPost] public async Task<IActionResult> AddImage (UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            await _mediator.Send(uploadProductImageCommandRequest);
            return Ok(true);
        }
        [HttpPost] public async Task<IActionResult> DeleteImage (RemoveProductImageCommandRequest removeProductImageCommandRequest)
        {
            await _mediator.Send(removeProductImageCommandRequest);
            return Redirect($"/editProduct/{removeProductImageCommandRequest.ProductId}");
        }
        [HttpPost] public async Task<IActionResult> SetAsMainImage (SetAsMainProductImageCommandRequest setAsMainProductImageCommandRequest)
        {
            await _mediator.Send(setAsMainProductImageCommandRequest);
            return Ok(true);
        }
        [HttpGet] public IActionResult Filters()
        {
            var model = _categoryService.GetAllWithFullDeep();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> DeleteFilterBox(string Id)
        {
            await _filterService.DeleteFilterBox(Id);
            return Redirect("/admin/filters");
        }
        [HttpGet] public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }
        [HttpGet] public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost] public async Task<IActionResult> AddRole (CreateRoleCommandRequest createRoleCommandRequest)
        {
            await _mediator.Send(createRoleCommandRequest);
            return Redirect("/admin/roles");
        }
        [HttpGet] public async Task<IActionResult> EditRole(string id)
        {
            var model = await _userService.GetRoleMembersWithNonMembers(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> AddToRole ([FromForm] string userId, [FromForm] string roleId)
        {
            await _userService.AddToRoleAsync(userId, roleId);
            return Redirect("/admin/editRole/" + roleId);
        }
        [HttpGet] public IActionResult Settings()
        {
            bool isOffline = PageSettings.isOffline;
            return View(isOffline);
        }
        [HttpPost] public IActionResult SaveSettings([FromForm] string isOffline, [FromForm] string siteMap)
        {
            PageSettings.ChangePageStatus(isOffline != null);
            if (siteMap != null) _sitemapService.CreateSitemap();

            return Redirect("/admin/settings");
        }
        [HttpPost] public IActionResult GetNotices()
        {
            var model = _pageStringService.GetNotices();
            return Ok(model);
        }
        [HttpPost] public async Task<IActionResult> SetNotice(UpdateNoticeReference data)
        {
            bool result = await _pageStringService.SetNotice(data);
            return Ok();
        }
        [HttpPost] public async Task<IActionResult> DeleteNotice(string id)
        {
            bool result = await _pageStringService.DeleteNoticeAsync(id);
            return Ok();
        }
        [HttpPost] public async Task<IActionResult> GetProductImages(string id)
        {
            var response = await _productImageService.GetByProductIdAsync(id);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> Search(string search)
        {
            var response = await _productService.Search(search);
            return Ok(response);
        }
        [HttpGet] public IActionResult Statistics()
        {
            return View();
        }
        [HttpPost] public async Task<IActionResult> GetStatistics(int year, int month)
        {
            var model = await _generalService.GetStatistics(year, month);
            return Ok(model);
        }
        [HttpGet] public async Task<IActionResult> Activesales()
        {
            var model = await _orderService.GetAllActiveOrdersWithDetails();
            return View(model);
        }
        [HttpGet] public async Task<IActionResult> Pastsales()
        {
            var model = await _orderService.GetAllPasteOrdersWithDetails();
            return View(model);
        }
        [HttpGet] public IActionResult LastLogs()
        {
            return View();
        }
        [HttpGet] public async Task<IActionResult> Cargo()
        {
            var model = await _shippingService.GetAllWithImages();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> ChangeOrderStatus(string orderId, bool isConfirmed,bool deliveryStatus)
        {
            await _orderService.UpdateOrderStatus
                                (orderId, isConfirmed, deliveryStatus);
            return Ok(200);
        }
        [HttpGet] public IActionResult AddFilter()
        {
            var model = _categoryService.GetAllWithFullDeep();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> AddFilter(CreateFilterCommandRequest createFilterCommandRequest)
        {
            await _mediator.Send(createFilterCommandRequest);
            return Redirect("/admin/filters");
        }
        [HttpGet] public async Task<IActionResult> EditFilter(string id)
        {
            var model = await _filterService.GetForEditById(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditFilter(string Id, string FilterBoxTitle, string[] CategoryIds)
        {
            await _filterService.UpdateFilterBox(Id, FilterBoxTitle, CategoryIds);
            return Redirect($"/admin/editFilter/{Id}");
        }
        [HttpPost] public async Task<IActionResult> UpdateFilter(string filterName, string filterId)
        {
            var response = await _filterService.UpdateFilterName(filterId, filterName);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> AddNewFilterToFilterBox(string filterBoxId, string filterName)
        {
            var response = await _filterService.AddNewFilterToFilterBox(filterBoxId, filterName);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> DeleteFilterFromFilterBox(string filterBoxId, string filterId)
        {
            var response = await _filterService.DeleteFilterFromFilterBox(filterBoxId, filterId);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> GetFiltersByFilterBoxId(string filterBoxId)
        {
            var response = await _filterService.GetFiltersByFilterBoxId(filterBoxId);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> GetFilterByCategory([FromForm] string[] CategoryIds)
        {
            var model = await _filterService.GetFilterByCategoryAsync(CategoryIds.ToList());
            return Ok(model);
        }
        [HttpGet] public async Task<IActionResult> FilterProducts(string id)
        {
            var model = await _filterService.GetFilterDetailsModelById(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> RemoveProductFromFilter(string productId, string filterId)
        {
            await _filterService.RemoveProductFromFilter(filterId, productId);
            return Redirect($"/admin/filterProducts/{filterId}");
        }
        [HttpPost] public async Task<IActionResult> SaveAlignment([FromForm] string alignmentJson, string productId)
        {
            var model = JsonConvert.DeserializeObject<List<ImageAndIndexsModel>>(alignmentJson);
            var response = await _productImageService.ReIndex(model, productId);
            return Ok();
        }
        [HttpPost] public async Task<IActionResult> UpdateSize(UpdateSizeCommandRequest updateSizeCommandRequest)
        {
            await _mediator.Send(updateSizeCommandRequest);
            return Ok();
        }
        [HttpPost] public async Task<IActionResult> DeleteOption(string sizeId, string productId)
        {
            var response = await _productService.DeleteSizeOnProduct(sizeId, productId);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> GetOptionsById(string productId)
        {
            List<ProductSize> response = await _productService.GetSizesById(productId);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> AddNewOption(string SizeName, string ProductId, double Price, int Stock)
        {
            var response = await _productService.AddNewOption(ProductId, SizeName, Price, Stock);
            return Ok(response);
        }
        [HttpGet] public async Task<IActionResult> EditShippingCompany(string id)
        {
            var model = await _shippingService.GetCompanyForEdit(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditShippingcompany(UpdateShippingCompanyRequest updateShippingCompanyRequest)
        {
            var response = await _mediator.Send(updateShippingCompanyRequest);
            return Redirect("/admin/cargo");
        }
        [HttpGet] public async Task<IActionResult> SaleDetails([FromQuery] string id)
        {
            var model = await _orderService.GetOrderDetailsById(id);
            ViewBag.OrderStatusus = await _orderService.GetAllOrderStatusus();
            ViewBag.ShippingCompanies = await _shippingService.GetAllWithImages();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> SaleDetails(string OrderId, string orderSatatusTitle, string shippingCompanyName)
        {
            await _orderService.UpdateOrderStatus(OrderId, orderSatatusTitle, shippingCompanyName);
            return Redirect($"/Admin/SaleDetails?id={OrderId}");
        }
        [HttpGet] public IActionResult AddShippingCompany()
        {
            return View();
        }
        [HttpPost] public async Task<IActionResult> AddShippingCompany(Shipping model)
        {
            await _shippingService.AddNewCompany(model);
            return Redirect("/admin/cargo");
        }
        [HttpPost] public async Task<IActionResult> DeleteShippingCompany(string Id)
        {
            await _shippingService.DeleteByIdAsync(Id);
            return Redirect("/admin/cargo");
        }
        [HttpGet] public async Task<IActionResult> MailInbox()
        {
            return View(await _generalService.GetAllSupportForms());
        }
        [HttpPost] public async Task<IActionResult> SendResponseMail(string MailId, string Subject, string MailResponse)
        {
            await _generalService.SendSupportMailResponse(MailId, Subject, MailResponse);
            return Redirect("/admin/mailInbox");
        }
        [HttpPost] public async Task<IActionResult> GetMailWithResponses(string mailId)
        {
            var response = await _generalService.GetMailWithResponses(mailId);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> GetFiltersByCategoryId(string CategoryIds)
        {
            return Ok(await _filterService.GetFilterByCategoryAsync(CategoryIds.Split(",").ToList()));
        }
        [HttpGet] public async Task<IActionResult> CommentsAndRates(string id)
        {
            var model = await _productService.GetCommentsAsync(id, true);
            return View(model);
        }
        [HttpGet] public async Task<IActionResult> CampaignManagement()
        {
            var model = _discountService.GetAllCapmaigns();
            return View(model);
        }
        [HttpGet] public IActionResult DiscountCodes()
        {
            var model = _discountService.GetAll();
            return View(model);
        }
        [HttpGet] public async Task<IActionResult> MailManagement()
        {
            return View();
        }
        [HttpGet] public IActionResult CreateDiscountCode()
        {
            var model = _categoryService.GetAllWithFullDeep();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> CreateDiscountCode(CreateDiscountCodeCommandRequest createDiscountCodeCommandRequest)
        {
            var response = await _mediator.Send(createDiscountCodeCommandRequest);
            return Redirect("/admin/discountcodes");
        }
        [HttpPost] public async Task<IActionResult> GetProductsByCategory(string CategoryIds)
        {
            var response = await _productService.GetProductsByCategoryIds(CategoryIds);
            return Ok(response);
        }
        [HttpGet] public async Task<IActionResult> EditDiscountCode(string id)
        {
            var model = await _discountService.GetForEditAsync(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditDiscountCode(EditDiscountCodeCommandRequest editDiscountCodeCommandRequest)
        {
            await _mediator.Send(editDiscountCodeCommandRequest);
            return Redirect($"/admin/editDiscountCode/{editDiscountCodeCommandRequest.Id}");
        }
        [HttpPost] public async Task<IActionResult> DeleteProductFromCode(string productId, string codeId)
        {
            await _discountService.DeleteProductFromCodeAsync(productId, codeId);
            return Redirect($"/admin/editDiscountCode/{codeId}");
        }
        [HttpGet] public IActionResult CreateCampaign()
        {
            var model = _categoryService.GetAllWithFullDeep();
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> CreateCampaign(CreateCampaignCommandRequest createDiscountCodeCommandRequest)
        {
            var response = await _mediator.Send(createDiscountCodeCommandRequest);
            return Redirect("/admin/campaignmanagement");
        }
        [HttpGet] public async Task<IActionResult> EditCampaign(string id)
        {
            var model = await _discountService.GetCampaignForEditAsync(id);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> EditCampaign(EditCampaignCommandRequest editCampaignCommandRequest)
        {
            await _mediator.Send(editCampaignCommandRequest);
            return Redirect($"/admin/editCampaign/{editCampaignCommandRequest.Id}");
        }
        [HttpPost] public async Task<IActionResult> CheckDiscountCode(string code)
        {
            var response = await _discountService.CheckDiscountCodeAsync(code, User.Identity.Name);
            return Ok(200);
        }
        [HttpPost] public async Task<IActionResult> RemoveDiscountCode()
        {
            var response = await _discountService.RemoveDiscountFromBasket(User.Identity.Name);
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> DeleteCampaign(string id)
        {
            await _discountService.DeleteCampaignAsync(id);
            return Redirect("/admin/campaignmanagement");
        }
        [HttpPost] public async Task<IActionResult> RemoveProductFromCampaign(string productId, string campaignId)
        {
            await _discountService.RemoveProductFromCampaignAsync(productId, campaignId);
            return Redirect("/admin/editCampaign");
        }
    }
}
