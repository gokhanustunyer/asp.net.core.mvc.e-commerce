using eticaret.data.Abstract.Cart;
using eticaret.data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Concrete.Cart
{
    public class CartRepository : Repository<et.Cart.Cart>, ICartRepository
    {
        public CartRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
