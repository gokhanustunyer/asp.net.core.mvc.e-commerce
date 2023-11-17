using eticaret.data.Abstract.Shipping;
using eticaret.data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Concrete.Shipping
{
    public class ShippingRepository : Repository<et.Cargo.Shipping>, IShippingRepository
    {
        public ShippingRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
