using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Offer
{
    public class DiscountCode: BaseEntity.BaseEntity
    {
        public string Code { get; set; }
        public int CodeLimitNumber { get; set; }
        public int DiscountRate { get; set; }
        public double DiscountNumber { get; set; }
        public DateTime CodeStartDate { get; set; }
        public DateTime CodeEndDate { get; set; }
        public List<Category.Category> Categories { get; set; }
        public List<Product.Product> Products { get; set; }
        public List<Cart.Cart>? Carts { get; set; }
    }
}
