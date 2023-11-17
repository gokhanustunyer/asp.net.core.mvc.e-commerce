using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Concrete.Product
{
    public class SizeRepository : Repository<et.Product.Size>, ISizeRepository
    {
        public SizeRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
