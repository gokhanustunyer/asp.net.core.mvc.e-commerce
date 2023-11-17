using eticaret.business.Abstract.Service;
using eticaret.business.Features.Commands.User.UpdateUserEmail;
using eticaret.business.Operations.OperationEntities;
using eticaret.data.Abstract.Order;
using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using eticaret.entity.Cart;
using eticaret.entity.EntityRefrences.CartReference;
using eticaret.entity.EntityRefrences.OrderReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.EntityRefrences.UserReference;
using eticaret.entity.Identity;
using eticaret.entity.Order;
using eticaret.entity.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Web;

namespace eticaret.business.Concrete.Service
{
    public class UserService : IUserService
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ETicaretDbContext _eTicaretDbContext;
        private readonly IEmailService _emailService;
        private readonly IProductService _productService;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;

        public UserService(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           ETicaretDbContext eTicaretDbContext,
                           RoleManager<AppRole> roleManager,
                           IProductService productService,
                           IEmailService emailService,
                           ICartService cartService,
                           IOrderRepository orderRepository,
                           IProductRepository productRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _eTicaretDbContext = eTicaretDbContext;
            _roleManager = roleManager;
            _productService = productService;
            _emailService = emailService;
            _cartService = cartService;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> AddToRoleAsync(string userId, string roleId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            AppRole role = await _roleManager.FindByIdAsync(roleId);
            await _userManager.AddToRoleAsync(user, role.Name);
            return true;
        }

        public async Task<bool> CreateAsync(CreateUserModel model)
        {
            AppUser user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Psw );
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string url = $"/Auth/ConfirmEmail?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";
            await _emailService.SendEmailAsync(user.Email, "Arite Hoş Geldiniz, Lütfen E-posta Adresinizi Doğrulayın", url);

            if (result.Succeeded)
                return true;
            return false;
        }

        public AppUser GetCommentsWithRateAsync(string username)
            => _eTicaretDbContext.Users
                                     .Include(u => u.Comments)
                                     .ThenInclude(c => c.Rate)
                                     .ThenInclude(r => r.Product)
                                     .ThenInclude(p => p.Categories)
                                     .FirstOrDefault(u => u.UserName == username);

        public async Task<AppUser> GetFullUserByName(string username)
                => await _eTicaretDbContext.Users
                                    .Include(u => u.Addresses)
                                    .FirstOrDefaultAsync(u => u.UserName == username);


        public async Task<(List<UserListModel>, List<UserListModel>, AppRole)>
            GetRoleMembersWithNonMembers(string roleId)
        {
            AppRole role = await _roleManager.FindByIdAsync(roleId);
            (List<UserListModel>, List<UserListModel>, AppRole) model = new(new(), new(), new());
            // Item1 => members, Item2 => nonmembers
            List<AppUser> users = _userManager.Users.ToList();
            model.Item3 = role;
            for (int i = 0; i < users.Count; i++)
            {
                if (await _userManager.IsInRoleAsync(users[i], role.Name))
                    model.Item1.Add(new UserListModel()
                    {
                         FirstName = users[i].FirstName,
                         LastName = users[i].LastName,
                         UserName = users[i].UserName,
                         Email = users[i].Email,
                         Id = users[i].Id,
                    });
                else
                {
                    model.Item2.Add(new UserListModel()
                    {
                        FirstName = users[i].FirstName,
                        LastName = users[i].LastName,
                        UserName = users[i].UserName,
                        Id = users[i].Id,
                    });
                }
            }
            return model;
        }

        public async Task<bool> LoginAsync(UserLoginModel model)
        {
            AppUser user;  SignInResult result = new();
            user = await _userManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                return false;    
            }
            if(!user.EmailConfirmed) 
            {
                return false;
            }
            result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
            return result.Succeeded;
        }

        public async Task<bool> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<string> ExternalLoginAsync()
        {
            string redirectUri;
            ExternalLoginInfo loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return "/login";
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult loginResult = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, true);
                if (loginResult.Succeeded)
                {
                    return "/";
                }
                else
                {
                    string email = loginInfo.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                    string firstName = loginInfo.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
                    string lastName = loginInfo.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname).Value;
                    string[] splittedName = firstName.Split(' ');
                    firstName = string.Join(" ", splittedName.SkipLast(1));
                    if (lastName == null)
                    {
                        lastName = splittedName[splittedName.Count() - 1];
                    }
                    AppUser user = new AppUser()
                    {
                        Email = email,
                        UserName = email,
                        FirstName = firstName,
                        LastName = lastName,
                        EmailConfirmed = true,
                        Id = Guid.NewGuid().ToString()
                    };
                    IdentityResult createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        IdentityResult addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
                        if (addLoginResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, true);
                            return "/";
                        }
                    }
                }
            }
            return "/login";
        }

        public async Task<List<ProductCommentCartModel>> GetProductsByUserComments(string username)
        {
            ProductComment comment;
            AppUser user = GetCommentsWithRateAsync(username);
            List<ProductCommentCartModel> result = new();
            for(int i = 0; i < user.Comments.Count(); i++)
            {
                comment = user.Comments.ToList()[i];
                result.Add(new() { Comment = comment, 
                                    Product = await _productService.
                                    ProductToListModel(comment.Product) });
            }
            return result;
        }

        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            IdentityResult result = new();
            AppUser user;
            if(userId != null && token != null)
            {
                user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;
                result = await _userManager.ConfirmEmailAsync(user, token);
                if( result.Succeeded )
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    await _cartService.CreateCart(user.UserName);
                    user.Cart = _cartService.GetByUserName(user.UserName);
                }
            }
            return result.Succeeded;
        }

        public async Task<bool> ConfirmUpdateEmail(string userId, string token, string newEmail)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.ChangeEmailAsync(user, newEmail, token);
            if (!result.Succeeded) 
            {
                return false;
            }
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);
            return true;
        }

        public async Task<UserCheckOutModel> GetCheckOut(string username)
        {
            UserCartModel cartModel = await _cartService.GetUsercartModel(username);
            AppUser? user = await _eTicaretDbContext.Users
                                                    .Include(u => u.Addresses)
                                                    .FirstOrDefaultAsync(u => u.UserName == username);
            return new() 
            {
                CartModel = cartModel,
                CartUser = user
            };
        }

        public async Task<UserOrderModel> GetOrdersAsync(string username)
        {
            UserOrderModel model = new() { PastOrders = new(), CurrentOrders = new() };
            AppUser user = await _orderRepository.Context.Users
                                          .Include(u => u.Orders)
                                          .ThenInclude(p => p.OrderItem)
                                          .Include(u => u.Orders)
                                          .FirstOrDefaultAsync(u => u.UserName == username);

            foreach (Order order in user.Orders)
            {
                OrderModel corder = new ()
                {
                    AddressTitle = order.AddressTitle,
                    PhoneNumber = order.PhoneNumber,
                    City = order.City,
                    District = order.District,
                    Neighborhood = order.Neighborhood,
                    PostCode = order.PostCode,
                    DetailedAddress = order.DetailedAddress,
                    CreateDate = order.CreateDate,
                    UpdateDate = order.UpdateDate,
                    DeliveryStatus = order.DeliveryStatus,
                    IsConfirmed = order.IsConfirmed,
                    Price = Math.Round(order.Price,2),
                    User = user
                };
                List<OrderItemModel> orderItemsModel = new();
                foreach(OrderItem item in order.OrderItem)
                {
                    Product pr = _productRepository.Table
                                                    .Include(p => p.ProductImages)
                                                    .Include(p => p.ProductSizes)
                                                    .FirstOrDefault
                                                    (p => p.Id.ToString() == item.ProductId);
                    if (pr == null) { continue; }
                    ProductListModel pr_model = await _productService.ProductToListModel(pr);
                    orderItemsModel.Add(new()
                    {
                        ProductId = item.ProductId,
                        SizeId = item.SizeId,
                        Size = pr.ProductSizes.FirstOrDefault
                               (s => s.SizeId.ToString() == item.SizeId),
                        Quantity = item.Quantity,
                        Product = pr_model,
                        TotalPrice = Math.Round(item.TotalPrice,2)
                    });
                }
                corder.OrderItem = orderItemsModel;
                if (order.DeliveryStatus == false)
                    model.CurrentOrders.Add(corder);
                else
                    model.PastOrders.Add(corder);
            }
            return model;
        }

        public async Task<bool> SendResetPasswordRequest(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return false;
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = $"/Auth/ResetPassword?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";
            await _emailService.SendEmailAsync(user.Email, "Şifretiniz Sıfırlayın - Arite", url);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
        {
            AppUser user;
            IdentityResult result;
            if(model.Password != model.PasswordAgain)
            {
                return false;
            }
            user = await _userManager.FindByIdAsync(model.userId);
            if (user == null)
            {
                return false;
            }
            result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            return result.Succeeded;
        }
    }
}
