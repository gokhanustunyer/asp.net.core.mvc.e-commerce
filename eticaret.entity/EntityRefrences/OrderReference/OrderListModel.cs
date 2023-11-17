using eticaret.entity.Cargo;
using eticaret.entity.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.OrderReference
{
    public class OrderListModel
    {
        public List<OrderModel> OrderModels { get; set; }
        public List<OrderStatus> OrderStatusus { get; set; }
        public List<Shipping> ShippingCompanies { get; set; }

    }
}
