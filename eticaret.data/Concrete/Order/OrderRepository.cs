using eticaret.data.Abstract.Order;
using eticaret.data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Concrete.Order
{
    public class OrderRepository : Repository<et.Order.Order>, IOrderRepository
    {
        public OrderRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
