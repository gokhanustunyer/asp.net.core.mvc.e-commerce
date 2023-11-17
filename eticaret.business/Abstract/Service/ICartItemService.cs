using eticaret.entity.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface ICartItemService
    {
        Task<bool> AddAsync(CartItem cartItem);
    }
}
