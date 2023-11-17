﻿using eticaret.data.Abstract.Order;
using eticaret.data.Contexts;
using eticaret.entity.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Concrete.Order
{
    public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
