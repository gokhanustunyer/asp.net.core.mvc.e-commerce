using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Logs;
using eticaret.data.Abstract.Order;
using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using eticaret.entity.Category;
using eticaret.entity.EntityRefrences.IdentityReference;
using eticaret.entity.EntityRefrences.MixReferences;
using eticaret.entity.EntityRefrences.PageEntities.Mail;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Order;
using eticaret.entity.PageEntities;
using eticaret.entity.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class GeneralService : IGeneralService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ETicaretDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IProductService _productService;
        private readonly ISizeRepository _sizeRepository;

        public GeneralService(IOrderRepository orderRepository,
                              IProductRepository productRepository,
                              ETicaretDbContext context,
                              IEmailService emailService,
                              IProductService productService,
                              ISizeRepository sizeRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _context = context;
            _emailService = emailService;
            _productService = productService;
            _sizeRepository = sizeRepository;
        }
        public async Task<List<SupportFormModel>> GetAllSupportForms()
            => await _context.SupportMails.OrderByDescending(s => s.CreateDate).ToListAsync();
        public async Task<SupportMailCombModel> GetMailWithResponses(string mailId)
        {
            SupportFormModel model = await _context.SupportMails.FirstOrDefaultAsync(s => s.Id.ToString() == mailId);
            List<SupportMailResponseModel> responses = _context.SupportMailResponses.Where(s => s.SupportMail == model).OrderBy(s => s.CreateDate).ToList();
            return new() { SupportForm = model, SupportFormResponses = responses };
        }
        public async Task<AdminStatisticsModel> GetStatistics(int year, int month)
        {
            List<ProductListModel> RecentlySoldItems = new();
            List<ProductListModel> TopSellingItems = new();
            List<SupportFormModel> LastUserMessages = new();
            List<ProductCommentModel> RecentlyReviews = new();
            List<UserModel> RecentRegisters = new();
            List<UserModel> RecentLogs = new();
            Dictionary<string,    int> VisitCountForaWeek = new();
            Dictionary<string,    int> OrderCountByCategoryName = new();
            Dictionary<string,    int> TotalSaleCountByDay = new();
            Dictionary<string, double> OrderPriceByCategoryName = new();
            Dictionary<string, double> TotalSalePriceByDay = new();
            Dictionary<string, double> statistics = new();
            AdminStatisticsModel adminStatisticsModel = new();
            List<DateTime> allDays = new();
            List<Order> lastOrders = new();
            List<ProductComment> productComments = new();
            int DailyVisitCount, DailyRegisterCount, DailyOrderCount; double DailySalary;
            
            List<Order> orders = _orderRepository.Table
                                                .Include(o => o.OrderItem)
                                                .Where(o => 
                                                o.CreateDate.Year == year &&
                                                o.CreateDate.Month == month)
                                                .OrderBy(o => o.CreateDate).ToList();
            DailyOrderCount = orders.Where(o => o.CreateDate.Day == DateTime.Now.Day).Count();
            DailyRegisterCount = _context.Users.Where(u => u.CreateDate.Year == year && u.CreateDate.Month == month && u.CreateDate.Day == DateTime.Now.Day).Count();
            DailyVisitCount = _context.PageLogs.Where(pl => pl.CreateDate.Year == year && pl.CreateDate.Month == month && pl.CreateDate.Day == DateTime.Now.Day).Count();
            DailySalary = Math.Round(orders.Where(o => o.CreateDate.Day == DateTime.Now.Day).Select(o => o.Price).Sum(),3);
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                allDays.Add(new DateTime(year, month, i));
            }
            for(int i = 0; i < allDays.Count; i++)
            {
                if (DateTime.Now.Day - allDays[i].Day < 8 && DateTime.Now.Day - allDays[i].Day > -1)
                {
                    
                    int visitCount = _context.PageLogs.Where(pl => pl.CreateDate.Year == year && pl.CreateDate.Month == month && pl.CreateDate.Day == allDays[i].Day).Count();
                    VisitCountForaWeek.Add($"{allDays[i].Day}.{allDays[i].Month}.{allDays[i].Year}", visitCount);
                }
                string key = $"{allDays[i].Day}.{allDays[i].Month}.{allDays[i].Year}";
                TotalSaleCountByDay.Add(key, 0);
                TotalSalePriceByDay.Add(key, 0);
            }
            foreach (Order order in orders)
            {
                int day = order.CreateDate.Day, count = order.OrderItem.Count;
                string key = $"{day}.{month}.{year}";
                double price = Math.Round(order.Price, 2);
                if (TotalSalePriceByDay.ContainsKey(key))
                {
                    TotalSalePriceByDay[key] += price;
                }
                else
                {
                    TotalSalePriceByDay.Add(key, price);
                }
                foreach (OrderItem item in order.OrderItem)
                {
                    Category? category = GetLastCategoryById(item.ProductId);
                    string? categoryKey = (category != null) ? category.Name : "Silinmiş";
                    if (OrderPriceByCategoryName.ContainsKey(categoryKey))
                    {
                        OrderPriceByCategoryName[categoryKey] += item.TotalPrice;
                    }
                    else
                    {
                        OrderPriceByCategoryName.Add(categoryKey, item.TotalPrice);
                    }
                    if (OrderCountByCategoryName.ContainsKey(categoryKey))
                    {
                        OrderCountByCategoryName[categoryKey] += item.Quantity;
                    }
                    else
                    {
                        OrderCountByCategoryName.Add(categoryKey, item.Quantity);
                    }
                    if (TotalSaleCountByDay.ContainsKey(key))
                    {
                        TotalSaleCountByDay[key] += item.Quantity;
                    }
                    else
                    {
                        TotalSaleCountByDay.Add(key, item.Quantity);
                    }
                }
            }
            lastOrders = _orderRepository.Table.Include(o => o.OrderItem).OrderByDescending(o => o.CreateDate).Take(3).ToList();
            foreach(Order order in lastOrders)
            {
                foreach(OrderItem orderItem in order.OrderItem)
                {
                    ProductListModel listModel = await _productService.GetByIdWithImage(orderItem.ProductId.ToString());
                    if (listModel != null)
                    {
                        Size selectedSize = await _sizeRepository.GetByIdAsync(orderItem.SizeId);
                        listModel.SelectedSizeName = selectedSize.Name;
                        listModel.SelectedSizePrice = orderItem.TotalPrice;
                        listModel.Quantity = orderItem.Quantity;
                        RecentlySoldItems.Add(listModel);
                    }
                }
            }
            
            LastUserMessages = _context.SupportMails.OrderByDescending(sf => sf.CreateDate).Take(3).ToList();
            RecentRegisters = _context.Users.OrderByDescending(u => u.CreateDate).Take(3).Select(u => new UserModel() { CreateDate = u.CreateDate, Email = u.Email, FirstName = u.FirstName, LastName = u.LastName, Gender = "Erkek", UserName = u.UserName, isExternal = false, Id = u.Id }).ToList();
            
            productComments = _context.ProductComments.Include(c => c.Rate).Include(c => c.User).Include(c => c.Product).OrderByDescending(c => c.CreateDate).Take(3).ToList();
            foreach(ProductComment item in productComments)
            {
                ProductCommentModel commentModel = new ProductCommentModel();
                commentModel.Comment = item.Comment;
                commentModel.FullName = item.User.FirstName + " " + item.User.LastName;
                commentModel.Product = await _productService.GetByIdWithImage(item.Product.Id.ToString());
                commentModel.Rate = item.Rate.Rate;
                commentModel.ProductId = commentModel.Product.Id.ToString();
                RecentlyReviews.Add(commentModel);
            }

            TopSellingItems = await _productService.GetMostSellings(3);

            adminStatisticsModel.RecentLogs = RecentLogs;
            adminStatisticsModel.RecentRegisters = RecentRegisters;
            adminStatisticsModel.RecentlyReviews = RecentlyReviews;
            adminStatisticsModel.LastUserMessages = LastUserMessages;
            adminStatisticsModel.TopSellingItems = TopSellingItems;
            adminStatisticsModel.RecentlySoldItems = RecentlySoldItems;
            adminStatisticsModel.VisitCountForaWeek = VisitCountForaWeek;
            adminStatisticsModel.TotalSalesPriceByDay = TotalSalePriceByDay;
            adminStatisticsModel.TotalSalesCountByDay = TotalSaleCountByDay;
            adminStatisticsModel.OrderCountByCategoryName = OrderCountByCategoryName;
            adminStatisticsModel.OrderPricesByCategoryName = OrderPriceByCategoryName;
            adminStatisticsModel.DailyOrderCount = DailyOrderCount;
            adminStatisticsModel.DailyVisitCount = DailyVisitCount;
            adminStatisticsModel.DailyRegisterCount = DailyRegisterCount;
            adminStatisticsModel.DailySalary = DailySalary;
            return adminStatisticsModel;
        }
        public async Task<bool> SendSupportMail(SupportFormModel formModel)
        {
            formModel.CreateDate = DateTime.Now;
            formModel.UpdateDate = DateTime.Now;
            await _context.SupportMails.AddAsync(formModel);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> SendSupportMailResponse(string mailId, string subject, string htmlContent)
        {
            SupportFormModel supportForm = await _context.SupportMails.FirstOrDefaultAsync(s => s.Id.ToString() == mailId);
            SupportMailResponseModel responseForm = new()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                Message = htmlContent,
                Subject = subject,
                SupportMail = supportForm
            };
            await _context.SupportMailResponses.AddAsync(responseForm);
            await _context.SaveChangesAsync();
            bool result = await _emailService.SendEmailAsHtml(supportForm.Email, subject, htmlContent);
            return result;
        }
        private Category GetLastCategoryById(string id) {
            Product? product = _productRepository.Table
                                         .Include(p => p.Categories)
                                         .FirstOrDefault(p =>
                                         p.Id.ToString() == id);
            Category? category = product?.Categories
                                         .OrderByDescending(c => c.CreateDate)
                                         .ToList()[0];

            return category;
        }
        private double GetPriceByProductIdAndSizeId(string productId, string sizeId)
        {
            Product product = _productRepository.Table.Include(p => p.ProductSizes)
                                                .FirstOrDefault(p => 
                                                p.Id.ToString() == productId);
            return Math.Round(product.ProductSizes
                          .FirstOrDefault
                          (ps => ps.SizeId.ToString()
                          == sizeId).Price,2);
        }
    }
}
