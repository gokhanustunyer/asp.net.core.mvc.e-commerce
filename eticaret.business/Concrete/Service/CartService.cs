using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Cart;
using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using eticaret.entity.Cart;
using eticaret.entity.EntityRefrences.CartReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Identity;
using eticaret.entity.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly ISizeRepository _sizeRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ETicaretDbContext _eTicaretDbContext;

        public CartService(ICartRepository cartRepository,
                           ICartItemRepository cartItemRepository,
                           IProductRepository productRepository,
                           IProductService productService,
                           IProductImageService productImageService,
                           ISizeRepository sizeRepository,
                           UserManager<AppUser> userManager,
                           ETicaretDbContext eTicaretDbContext)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _productService = productService;
            _productImageService = productImageService;
            _sizeRepository = sizeRepository;
            _userManager = userManager;
            _eTicaretDbContext = eTicaretDbContext;
        }

        public async Task<bool> AddToCart(string productId, string sizeId, int quantity, string username)
        {
            Cart cart = GetByUserName(username);
            if (cart == null)
            {
                CreateCart(username);
                cart = GetByUserName(username);
            }
            cart.UpdateDate = DateTime.Now;
            CartItem? cartItem = await _cartRepository.Context.CartItems.Include(c => c.Cart)
                .FirstOrDefaultAsync(ci => ci.Cart.Id == cart.Id && ci.ProductId == productId);

            if (cartItem != null)
                cartItem.Quantity++;
            else
                _cartRepository.Context.CartItems.Add(new()
                {
                    Id = Guid.NewGuid(),
                    Quantity = quantity,
                    ProductId = productId,
                    SizeId = sizeId,
                    UpdateDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    Cart = cart
                });
            

            await _cartRepository.Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateCart(string username)
        {
            AppUser user = await _userManager.FindByNameAsync(username);
            await _cartRepository.AddAsync(new() 
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                User = user,
            });
            await _cartRepository.SaveAsync();
            return true;
        }

        public async Task<bool> ExtractFromCartItems(Cart cart, string productId, string sizeId)
        {
            CartItem cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId && ci.SizeId == sizeId);
            cart.CartItems.Remove(cartItem);
            await _cartRepository.SaveAsync();
            return true;
        }

        public Cart GetByUserName(string username)
        {
            Cart cart = _eTicaretDbContext.Users
                                          .Include(u => u.Cart)
                                          .ThenInclude(c => c.DiscountCode)
                                          .Include(u => u.Cart)
                                          .ThenInclude(c => c.CartItems)
                                          .FirstOrDefault(u => u.UserName == username).Cart;
            return cart;
        }

        public async Task<UserCartModel> GetUsercartModel(string username)
        {
            int totalQuantity = 0;
            double totalPrice = 0;
            Cart cart = GetByUserName(username);

            if (cart == null)
            {
                await CreateCart(username);
                cart = GetByUserName(username);
            }

            List<UserCartItem> userCartItems = new();
            for (var i = 0; i < cart.CartItems.Count; i++) 
            {
                Product? product = await _productRepository.Table.Include(p => p.ProductSizes)
                                                                 .Include(p => p.Campaigns)
                                                                 .FirstOrDefaultAsync(p => p.Id.ToString() == cart.CartItems[i].ProductId);
                ProductListModel productListModel = await _productService.ProductToListModel(product);
                productListModel.Price = Math.Round(product.ProductSizes.FirstOrDefault
                                         (s => s.SizeId.ToString() ==
                                         cart.CartItems[i].SizeId).Price,2);

                double price = (double)(productListModel.DiscountedPrice != null ? productListModel.DiscountedPrice : productListModel.Price);
                totalQuantity += cart.CartItems[i].Quantity;
                totalPrice += price * cart.CartItems[i].Quantity;
                Size sizeName = await _sizeRepository.GetByIdAsync(cart.CartItems[i].SizeId);
                userCartItems.Add(new() {
                    Price = price,
                    SizeId = cart.CartItems[i].SizeId,
                    ProductId = cart.CartItems[i].ProductId,
                    Quantity = cart.CartItems[i].Quantity,
                    Product = productListModel,
                    SizeName = sizeName.Name,
                });
            }

            UserCartModel userCartModel = new() { 
                CartItems = userCartItems,
                totalQuantity = totalQuantity,
                totalPrice = totalPrice,
                DiscountCode = cart.DiscountCode
            };

            return userCartModel;
        }

        public async Task<bool> IncreaseFromBasket(string userName, string productId, string sizeId, int quantity=1)
        {
            Cart cart = GetByUserName(userName);
            if(cart == null)
            {
                CreateCart(userName);
                cart = GetByUserName(userName);
            }
            CartItem cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId
                                == productId && ci.SizeId == sizeId);
            if (cartItem == null)
            {
                cartItem = new();
                cartItem.ProductId = productId;
                cartItem.SizeId = sizeId;
                cartItem.CreateDate = DateTime.Now;
                cartItem.UpdateDate = DateTime.Now;
                cartItem.Cart = cart;
                cartItem.Quantity = quantity;
                await _cartItemRepository.AddAsync(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                _cartItemRepository.Update(cartItem);
            }
            await _cartItemRepository.SaveAsync();
            return true;
        }

        public async Task<bool> MinusFromCartItems(string userName, string productId, string sizeId)
        {
            Cart cart = GetByUserName(userName);
            CartItem cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId 
                                == productId && ci.SizeId == sizeId);
            cartItem.Quantity--;
            if (cartItem.Quantity < 1)
                await _cartItemRepository.RemoveAsync(cartItem.Id.ToString());
            else
                _cartItemRepository.Update(cartItem);

            await _cartItemRepository.SaveAsync();
            return true;
        }
    }
}
