using eticaret.business.Abstract.Service;
using eticaret.business.Features.Commands.Cart.Checkout;
using eticaret.business.Features.Commands.Product.GetComment;
using eticaret.business.Features.Commands.User.AddAddress;
using eticaret.business.Features.Commands.User.DeleteAddress;
using eticaret.business.Features.Commands.User.UpdateUser;
using eticaret.business.Features.Commands.User.UpdateUserEmail;
using eticaret.business.Features.Commands.User.UpdateUserPassword;
using eticaret.entity.Cart;
using eticaret.entity.EntityRefrences.CartReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.EntityRefrences.UserReference;
using eticaret.entity.Identity;
using eticaret.entity.Product;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eticaret.webui.Controllers
{
    public class UserController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IMediator _mediator;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly ICityDbService _cityDbService;
        private readonly IProductService _productService;
        private readonly IDiscountService _discountService;
        public UserController(ICartService cartService,
                              IMediator mediator,
                              UserManager<AppUser> userManager,
                              IUserService userService,
                              ICityDbService cityDbService,
                              IProductService productService,
                              IDiscountService discountService)
        {
            _cartService = cartService;
            _mediator = mediator;
            _userManager = userManager;
            _userService = userService;
            _cityDbService = cityDbService;
            _productService = productService;
            _discountService = discountService;
        }

        [HttpGet] public IActionResult Index()
        {
            return View();
        }
        [HttpGet] public IActionResult Favorites()
        {
            return View();
        }
        [HttpGet] public async Task<IActionResult> Cart()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/login");
            UserCartModel model = await _cartService.GetUsercartModel(User.Identity.Name);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> DeleteFromBasket([FromForm] string productId,[FromForm] string sizeId)
        {
            Cart cart = _cartService.GetByUserName(User.Identity.Name);
            await _cartService.ExtractFromCartItems(cart, productId, sizeId);
            return Redirect("/user/cart");
        }
        [HttpPost] public async Task<IActionResult> MinusFromBasket([FromForm] string productId, [FromForm] string sizeId)
        {
            await _cartService.MinusFromCartItems(User.Identity.Name, productId, sizeId);
            return Ok(200);
        }
        [HttpPost] public async Task<IActionResult> IncreaseFromBasket([FromForm] string productId, [FromForm] string sizeId)
        {
            await _cartService.IncreaseFromBasket(User.Identity.Name, productId, sizeId);
            return Ok(200);
        }
        [HttpPost] public async Task<IActionResult> AddToCart(string productId, string sizeId, int quantity)
        {
            string username = User.Identity.Name;
            if (username == null) { return Redirect("/login"); }
            bool result = await _cartService.IncreaseFromBasket(username, productId, sizeId,quantity);
            var pr = await _productService.GetByIdWithImage(productId);
            return Redirect($"/giyim/{pr.Url}");
        }
        [HttpPost] public async Task<IActionResult> BuyNow(string productId, string sizeId, int quantity)
        {
            string username = User.Identity.Name;
            if (username == null) { return Redirect("/login"); }
            bool result = await _cartService.IncreaseFromBasket(username, productId, sizeId, quantity);
            return Redirect("/user/cart");
        }
        [HttpGet] public async Task<IActionResult> CheckOut()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/login");
            UserCheckOutModel model = await _userService.GetCheckOut(User.Identity.Name);
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> CheckOut(CheckoutCommandRequest checkoutCommandRequest)
        {
            checkoutCommandRequest.UserName = User.Identity.Name;
            await _mediator.Send(checkoutCommandRequest);
            return Redirect("/user/orders");
        }
        [HttpPost] public async Task<IActionResult> GetComment(GetCommentCommandRequest getCommandRequest)
        {
            if (User.Identity.Name == null)
                return Redirect("/login");
            getCommandRequest.UserName = User.Identity.Name;
            await _mediator.Send(getCommandRequest);
            return Redirect("/");
        }
        [HttpGet] public async Task<IActionResult> Information()
        {
            AppUser user = await _userService.GetFullUserByName(User.Identity.Name);
            return View(user);
        }
        [HttpGet] public async Task<IActionResult> EditUserInfo()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }
        [HttpPost] public IActionResult GetAllCities()
        {
            return Ok(_cityDbService.GetAllCities());
        }
        [HttpPost] public IActionResult GetDistrict(int id)
        {
            return Ok(_cityDbService.GetAllDistrictByCityId(id));
        }
        [HttpPost] public IActionResult GetNeighborhood(int id)
        {
            return Ok(_cityDbService.GetAllNeighborhoodsByDistrict(id));
        }
        [HttpPost] public async Task<IActionResult> UpdateUser(UpdateUserCommandRequest updateUserCommandRequest)
        {
            updateUserCommandRequest.userName = User.Identity.Name;
            var response = await _mediator.Send(updateUserCommandRequest);
            return Redirect("/user/information");
        }
        [HttpPost] public async Task<IActionResult> AddAddress(AddAddressCommandRequest addAddressCommandRequest)
        {
            addAddressCommandRequest.UserName = User.Identity.Name;
            return Ok(await _mediator.Send(addAddressCommandRequest));
        }
        [HttpPost] public async Task<IActionResult> DeleteAddress(DeleteAddressCommandRequest deleteAddressCommandRequest)
        {
            return Ok(await _mediator.Send(deleteAddressCommandRequest));
        }
        [HttpPost] public async Task<IActionResult> GetAddresses()
        {
            var response = await _userService.GetFullUserByName(User.Identity.Name);
            return Ok(response.Addresses.Select(a => new Address(){ City = a.City, District = a.District, Neighborhood = a.Neighborhood, Title = a.Title, Id = a.Id, DetailedAddress = a.DetailedAddress } ));
        }
        [HttpGet] public async Task<IActionResult> Comments()
        {
            var userWithComments = await _userService.GetProductsByUserComments(User.Identity.Name);
            return View(userWithComments);
        }
        [HttpGet] public async Task<IActionResult> Orders()
        {
            var model = await _userService.GetOrdersAsync(User.Identity.Name);
            return View(model);
        }
        [HttpGet] public async Task<IActionResult> UpdateEmail()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }
        [HttpPost] public async Task<IActionResult> UpdateEmail(UpdateUserEmailCommandRequest updateUserEmailCommandRequest)
        {
            updateUserEmailCommandRequest.UserName = User.Identity.Name;
            await _mediator.Send(updateUserEmailCommandRequest);
            return Redirect("/user/information");
        }
        [HttpGet] public async Task<IActionResult> ConfirmUpdateEmail(string userId, string token, string newEmail)
        {
            var response = await _userService.ConfirmUpdateEmail(userId, token, newEmail);
            return Redirect("/user/information");
        }
        [HttpGet] public async Task<IActionResult> UpdatePassword([FromQuery] string token)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["PasswordResetToken"] = token;
            return View(user);
        }
        [HttpPost] public async Task<IActionResult> UpdatePassword(UpdateUserPasswordCommandRequest updateUserPasswordCommandRequest)
        {
            updateUserPasswordCommandRequest.UserName = User.Identity.Name;
            await _mediator.Send(updateUserPasswordCommandRequest);
            return Redirect("/user/information");
        }
        [HttpPost] public async Task<IActionResult> UpdatePasswordRequest(SendUpdateUserPasswordMailCommandRequest updateUserPasswordCommandRequest)
        {
            if (updateUserPasswordCommandRequest == null) 
                { updateUserPasswordCommandRequest = new(); }
            updateUserPasswordCommandRequest.UserName = User.Identity.Name;
            await _mediator.Send(updateUserPasswordCommandRequest);
            return Ok(200);
        }
        [HttpPost] public async Task<IActionResult> NewProductQA([FromForm] ProductQaCreateModel formModel)
        {
            if (!User.Identity.IsAuthenticated) { return Redirect("/login"); }
            formModel.UserName = User.Identity.Name;
            Product product = await _productService.CreateQa(formModel);
            return Redirect($"/urunler/{product.Url}");
        }
        [HttpPost] public async Task<IActionResult> SendQaResponse(string QAResponse, string productId, string qaId)
        {
            if (!User.Identity.IsAuthenticated) { return Redirect("/login"); }
            Product product = await _productService.SendQaResponse(QAResponse, productId, qaId, User.Identity.Name);
            return Redirect($"/urunler/{product.Url}");
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
    }
}
