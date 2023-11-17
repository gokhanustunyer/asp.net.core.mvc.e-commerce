using eticaret.entity.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.CartReference
{
    public class UserCartModel
    {
        public double totalPrice { get; set; }
        public int totalQuantity { get; set; }
        public List<UserCartItem> CartItems { get; set; }
        public DiscountCode? DiscountCode { get; set; }
    }
}
