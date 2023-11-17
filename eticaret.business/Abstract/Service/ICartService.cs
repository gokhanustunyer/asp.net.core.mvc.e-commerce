using eticaret.entity.Cart;
using eticaret.entity.EntityRefrences.CartReference;
using eticaret.entity.EntityRefrences.CategoryReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface ICartService
    {
        Cart GetByUserName(string username);
        Task<bool> AddToCart(string productId, string sizeId, int quantity, string username);
        Task<bool> CreateCart(string username);
        Task<UserCartModel> GetUsercartModel(string username);
        Task<bool> ExtractFromCartItems(Cart cart, string productId, string sizeId);
        Task<bool> MinusFromCartItems(string userId, string productId, string sizeId);
        Task<bool> IncreaseFromBasket(string userId, string productId, string sizeId, int quantity = 1);
    }
}
