using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Product
{
    public class ProductImage: File
    {
        public List<Product> Product { get; set; }
        public int? Index { get; set; }
    }
}
