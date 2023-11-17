using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using eticaret.entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Concrete.Product
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
