using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.OrderReference
{
    public class UserOrderModel
    {
        public List<OrderModel> CurrentOrders { get; set; }
        public List<OrderModel> PastOrders { get; set; }
    }
}
