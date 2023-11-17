using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.CartReference
{
    public class UserCartItem
    {
        public string ProductId { get; set; }
        public string SizeId { get; set; }
        public string SizeName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public ProductListModel Product { get; set; }
    }
}
