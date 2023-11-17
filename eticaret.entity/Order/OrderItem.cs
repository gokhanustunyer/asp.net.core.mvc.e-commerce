using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Order
{
    public class OrderItem: BaseEntity.BaseEntity
    {
        public string ProductId { get; set; }
        public string SizeId { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
    }
}
