using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.Cart
{
    public class CartItem: BaseEntity.BaseEntity
    {
        public string ProductId { get; set; }
        public string SizeId { get; set; }
        public int Quantity { get; set; }
        public Cart Cart { get; set; }
    }
}
