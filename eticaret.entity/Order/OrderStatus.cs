using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.Order
{
    public class OrderStatus: et.BaseEntity.BaseEntity
    {
        public string Title { get; set; }
    }
}
