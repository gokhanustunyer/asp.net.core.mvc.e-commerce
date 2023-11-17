using eticaret.entity.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Abstract.Order
{
    public interface IOrderStatusRepository: IRepository<OrderStatus>
    {
    }
}
