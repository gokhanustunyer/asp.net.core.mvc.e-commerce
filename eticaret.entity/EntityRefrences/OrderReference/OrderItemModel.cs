using eticaret.entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.OrderReference
{
    public class OrderItemModel
    {
        public string ProductId { get; set; }
        public string SizeId { get; set; }
        public ProductSize Size { get; set; }
        public ProductReference.ProductListModel Product { get; set; }
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
