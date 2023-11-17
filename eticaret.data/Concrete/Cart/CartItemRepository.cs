using eticaret.data.Abstract.Cart;
using eticaret.data.Contexts;
using eticaret.entity.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Concrete.Cart
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
