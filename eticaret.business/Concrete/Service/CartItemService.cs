using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Cart;
using eticaret.entity.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class CartItemService: ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<bool> AddAsync(CartItem cartItem)
        {
            await _cartItemRepository.AddAsync(cartItem);
            return true;
        }
    }
}
