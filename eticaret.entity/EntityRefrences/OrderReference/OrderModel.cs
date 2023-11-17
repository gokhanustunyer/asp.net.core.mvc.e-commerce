using eticaret.entity.Cargo;
using eticaret.entity.Identity;
using eticaret.entity.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.OrderReference
{
    public class OrderModel
    {
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public AppUser User { get; set; }
        public string? OrderId { get; set; }
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
        public OrderStatus OrderStatus { get; set; }
        public Shipping ShippingCompany { get; set; }
        public List<OrderItemModel> OrderItem { get; set; }
    }
}
