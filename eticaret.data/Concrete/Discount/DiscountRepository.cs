using eticaret.data.Abstract.Discount;
using eticaret.data.Contexts;
using eticaret.entity.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Concrete.Discount
{
    public class DiscountRepository : Repository<DiscountCode>, IDiscountRepository
    {
        public DiscountRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
