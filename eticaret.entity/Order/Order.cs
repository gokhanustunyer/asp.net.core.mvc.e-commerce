using eticaret.entity.Cargo;
using eticaret.entity.Cart;
using eticaret.entity.Identity;
using eticaret.entity.Product;
using eticaret.entity.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Order
{
    public class Order: BaseEntity.BaseEntity
    {
        public AppUser User { get; set; }
        public string? PhoneNumber { get; set; }
        public string AddressTitle { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string DetailedAddress { get; set; }
        public string? PostCode { get; set; }
        public double Price { get; set; }
        public bool IsConfirmed { get; set; }
        public bool DeliveryStatus { get; set; }
        public double? DiscountAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderItem> OrderItem { get; set; }
        public Shipping? ShippingCompany { get; set; }
    }
}
