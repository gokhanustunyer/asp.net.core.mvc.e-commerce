using eticaret.business.Abstract.Service;
using eticaret.business.Features.Commands.Auth.Login;
using eticaret.entity.EntityRefrences.UserReference;
using eticaret.entity.Identity;
using eticaret.webui.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace eticaret.webui.Controllers
{
    //[AutoValidateAntiforgeryToken]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;

        public AuthController(IUserService userService,
                              ICartService cartService,
                              SignInManager<AppUser> signInManager,
                              UserManager<AppUser> userManager,
                              IMediator mediator)
        {
            _userService = userService;
            _cartService = cartService;
            _signInManager = signInManager;
            _userManager = userManager;
            _mediator = mediator;
        }


        [HttpGet] public IActionResult Login()
        {
            return View();
        }

        [HttpPost] public async Task<IActionResult> Login(LoginCommandRequest loginCommandRequest)
        {
            TempData["Alert"] = JsonConvert.SerializeObject((await _mediator.Send(loginCommandRequest)).Notice);
            return Redirect("/");
        }

        [HttpGet] public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return Redirect("/");
        }

        [HttpGet] public IActionResult Register()
        {
            return View();
        }

        [HttpPost] public async Task<IActionResult> Register(CreateUserModel model)
        {
            await _userService.CreateAsync(model);
            return Redirect("/login");
        }

        [HttpGet] public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet] public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            bool result = await _userService.ConfirmEmail(userId, token);
            return View(result);
        }

        [HttpPost] public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
            var callBackUrl = Url.Action("ExternalLoginCallback");
            properties.RedirectUri = callBackUrl;
            return Challenge(properties, provider);
        }

        [HttpGet] public async Task<IActionResult> ExternalLoginCallback()
        {
            return Redirect(await _userService.ExternalLoginAsync());
        }

        [HttpGet] public IActionResult ForgotPassword()
        { 
            return View();
        }

        [HttpPost] public async Task<IActionResult> ForgotPassword(string email)
        {
            bool resetPswResult = await _userService.SendResetPasswordRequest(email);
            if (resetPswResult)
                TempData["Validations"] = "E-Posta Adresinize Gönderilen Maildeki Linke Giderek Şifrenizi Yenileyebilirsiniz";
            else
                TempData["Validations"] = "Girdiğiniz E-Posta Adresi ile Eşleşen Bir Kayıda Rastlamadık";
            return Redirect("/auth/forgotPassword");
        }

        [HttpGet] public IActionResult ResetPassword(string userId, string token)
        {
            var model = new ResetPasswordModel() { userId = userId, Token = token };
            if(model.userId == null || model.Token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost] public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            await _userService.ResetPasswordAsync(model);
            return Redirect("/login");
        }
    }
}
