using eticaret.entity.EntityRefrences.OrderReference;
using eticaret.entity.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface IOrderService
    {
        Task<OrderListModel> GetAllActiveOrdersWithDetails();
        Task<OrderListModel> GetAllPasteOrdersWithDetails();
        Task<bool> UpdateOrderStatus(string orderId, bool isConfirmed, bool deliveryStatus);
        Task<OrderModel> GetOrderDetailsById(string orderId);
        Task<List<OrderStatus>> GetAllOrderStatusus();
        Task<bool> UpdateOrderStatus(string OrderId, string orderSatatusTitle, string shippingCompanyName);
    }
}
