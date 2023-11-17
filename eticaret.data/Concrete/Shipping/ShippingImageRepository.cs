using eticaret.data.Abstract.Shipping;
using eticaret.data.Contexts;
using eticaret.entity.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Concrete.Shipping
{
    public class ShippingImageRepository : Repository<ShippingImage>, IShippingImageRepository
    {
        public ShippingImageRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
