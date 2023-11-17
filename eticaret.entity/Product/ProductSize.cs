
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Product
{
    public class ProductSize
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid SizeId { get; set; }
        public Size Size { get; set; }
        public string SizeName { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public double? DiscountedPrice { get; set; }
    }
}
