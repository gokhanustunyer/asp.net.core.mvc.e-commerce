using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.Cargo
{
    public class Shipping: BaseEntity.BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public bool IsDefault { get; set; }
        public List<ShippingImage>? ShippingImages { get; set; }
    }
}
