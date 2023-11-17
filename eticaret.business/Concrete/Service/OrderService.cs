using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Order;
using eticaret.data.Abstract.Product;
using eticaret.data.Abstract.Shipping;
using eticaret.entity.Cargo;
using eticaret.entity.EntityRefrences.OrderReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Order;
using eticaret.entity.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository    _orderRepository;
        private readonly IProductService      _productService;
        private readonly IProductRepository _productRepository;
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IShippingRepository _shippingRepository;

        public OrderService(IOrderRepository orderRepository,
                            IProductService productService,
                            IProductRepository productRepository,
                            IOrderStatusRepository orderStatusRepository,
                            IShippingRepository shippingRepository)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _productRepository = productRepository;
            _orderStatusRepository = orderStatusRepository;
            _shippingRepository = shippingRepository;
        }
        public async Task<OrderListModel> GetAllActiveOrdersWithDetails()
        {
            List<Order> allOrders = _orderRepository.Table
                                                    .Include(o => o.OrderItem)
                                                    .Include(o => o.User)
                                                    .Include(o => o.ShippingCompany)
                                                    .Include(o => o.OrderStatus)
                                                    .Where(o => !o.DeliveryStatus)
                                                    .OrderBy(o => o.CreateDate)
                                                    .ToList();
            return new() { OrderModels = await OrdersToOrderModelList(allOrders), OrderStatusus = _orderStatusRepository.GetAll().ToList(), ShippingCompanies = _shippingRepository.GetAll().ToList()};
        }
        public async Task<OrderListModel> GetAllPasteOrdersWithDetails()
        {
            List<Order> allOrders = _orderRepository.Table
                                                    .Include(o => o.OrderItem)
                                                    .Include(o => o.User)
                                                    .Include(o => o.ShippingCompany)
                                                    .Include(o => o.OrderStatus)
                                                    .Where(o => o.DeliveryStatus)
                                                    .OrderByDescending(o => o.CreateDate)
                                                    .ToList();
            return new() { OrderModels = await OrdersToOrderModelList(allOrders), OrderStatusus = _orderStatusRepository.GetAll().ToList(), ShippingCompanies = _shippingRepository.GetAll().ToList() };
        }
        public async Task<bool> UpdateOrderStatus(string orderId, bool isConfirmed, bool deliveryStatus)
        {
            Order order = await _orderRepository.GetByIdAsync(orderId);
            order.IsConfirmed = isConfirmed;
            order.DeliveryStatus = deliveryStatus;
            _orderRepository.Update(order);
            await _orderRepository.SaveAsync();
            return true;
        }
        private async Task<List<OrderModel>> OrdersToOrderModelList(List<Order> allOrders)
        {
            List<OrderModel> model = new();
            foreach (Order order in allOrders)
            {
                model.Add(await OrderToOrderModel(order));
            }
            return model;
        }
        private async Task<OrderModel> OrderToOrderModel(Order order)
        {
            OrderModel corder = new()
            {
                AddressTitle = order.AddressTitle,
                PhoneNumber = order.PhoneNumber,
                City = order.City,
                District = order.District,
                Neighborhood = order.Neighborhood,
                PostCode = order.PostCode,
                DetailedAddress = order.DetailedAddress,
                CreateDate = order.CreateDate,
                UpdateDate = order.UpdateDate,
                DeliveryStatus = order.DeliveryStatus,
                IsConfirmed = order.IsConfirmed,
                Price = Math.Round(order.Price, 2),
                User = order.User,
                OrderStatus = order.OrderStatus,
                OrderId = order.Id.ToString(),
                ShippingCompany = order.ShippingCompany
            };
            List<OrderItemModel> orderItemsModel = new();
            foreach (OrderItem item in order.OrderItem)
            {
                Product pr = _productRepository.Table
                                                .Include(p => p.ProductImages)
                                                .Include(p => p.ProductSizes)
                                                .FirstOrDefault
                                                (p => p.Id.ToString() == item.ProductId);
                if (pr == null) { continue; }
                ProductListModel pr_model = await _productService.ProductToListModel(pr);
                orderItemsModel.Add(new()
                {
                    ProductId = item.ProductId,
                    SizeId = item.SizeId,
                    Size = pr.ProductSizes.FirstOrDefault
                           (s => s.SizeId.ToString() == item.SizeId),
                    Quantity = item.Quantity,
                    Product = pr_model,
                    TotalPrice = Math.Round(item.TotalPrice, 2)
                });
            }
            corder.OrderItem = orderItemsModel;
            return corder;
        }
        public async Task<OrderModel> GetOrderDetailsById(string orderId)
        {
            Order order = await _orderRepository.Table.Include(o => o.User)
                                                      .Include(o => o.OrderItem)
                                                      .Include(o => o.ShippingCompany)
                                                      .Include(o => o.OrderStatus)
                                                      .FirstOrDefaultAsync(o => o.Id.ToString() == orderId);
            return await OrderToOrderModel(order);
        }
        public async Task<List<OrderStatus>> GetAllOrderStatusus()
            => await _orderStatusRepository.GetAll().ToListAsync();
        public async Task<bool> UpdateOrderStatus(string OrderId, string orderSatatusTitle, string shippingCompanyName)
        {
            Order order = await _orderRepository.Table.Include(o => o.OrderStatus).FirstOrDefaultAsync(o => o.Id.ToString() == OrderId);
            OrderStatus orderStatus = await _orderStatusRepository.Table.FirstOrDefaultAsync(os => os.Title == orderSatatusTitle);
            Shipping shippingCompany = await _shippingRepository.Table.FirstOrDefaultAsync(sc => sc.Name == shippingCompanyName);
            order.OrderStatus = orderStatus;
            order.ShippingCompany = shippingCompany;
            _orderRepository.Update(order);
            await _orderRepository.SaveAsync();
            return true;
        }
    }
}
