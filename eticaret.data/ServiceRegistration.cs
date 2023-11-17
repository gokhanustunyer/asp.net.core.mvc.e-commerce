using eticaret.data.Abstract;
using eticaret.data.Abstract.Cart;
using eticaret.data.Abstract.Category;
using eticaret.data.Abstract.Discount;
using eticaret.data.Abstract.File;
using eticaret.data.Abstract.Filter;
using eticaret.data.Abstract.Logs;
using eticaret.data.Abstract.Order;
using eticaret.data.Abstract.PageStrings;
using eticaret.data.Abstract.Product;
using eticaret.data.Abstract.Shipping;
using eticaret.data.Concrete.Cart;
using eticaret.data.Concrete.Category;
using eticaret.data.Concrete.Discount;
using eticaret.data.Concrete.File;
using eticaret.data.Concrete.Filter;
using eticaret.data.Concrete.Logs;
using eticaret.data.Concrete.Order;
using eticaret.data.Concrete.PageStrings;
using eticaret.data.Concrete.Product;
using eticaret.data.Concrete.Shipping;
using eticaret.data.Contexts;
using eticaret.data.Services.Abstract;
using eticaret.data.Services.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eticaret.data
{
    public static class ServiceRegistration
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            services.AddDbContext<ETicaretDbContext>(options => options.UseMySql("server=localhost;port=3306;user=root;password=gokhan949;database=eticaretdb;", serverVersion));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<IPageStringsRepository, PageStringsRepository>();
            services.AddScoped<ICitiesDbService, CitiesDbService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IFilterBoxRepository, FilterBoxRepository>();
            services.AddScoped<IFilterRepository, FilterRepository>();
            services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
            services.AddScoped<IShippingRepository, ShippingRepository>();
            services.AddScoped<IShippingImageRepository, ShippingImageRepository>();
            services.AddScoped<IPageLogRepository, PageLogRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
        }
    }
}
