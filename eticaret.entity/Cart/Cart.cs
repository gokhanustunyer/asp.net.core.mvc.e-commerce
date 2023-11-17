using eticaret.entity.Identity;
using eticaret.entity.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Cart
{
    public class Cart: BaseEntity.BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public List<CartItem> CartItems { get; set; }
        public DiscountCode? DiscountCode { get; set; }
    }
}
