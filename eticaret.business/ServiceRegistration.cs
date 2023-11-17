using eticaret.business.Abstract.Service;
using eticaret.business.Abstract.Storage.Azure;
using eticaret.business.Concrete.Service;
using eticaret.business.Concrete.Storage.Azure;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eticaret.business.Abstract.Operations;
using eticaret.business.Concrete.Operations;
using eticaret.business.Operations.OperationEntities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using eticaret.business.Concrete.Storage.Local;
using eticaret.business.Abstract.Storage.Local;

namespace eticaret.business
{
    public static class ServiceRegistration
    {
        public static void AddBusinessServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(ServiceRegistration));
            serviceCollection.AddMediatR(typeof(ServiceRegistration));
            serviceCollection.AddScoped<IAzureStorage, AzureStorage>();
            serviceCollection.AddScoped<ILocalStorage, LocalStorage>();
            serviceCollection.AddScoped<IProductService, ProductService>();
            serviceCollection.AddScoped<ICategoryService, CategoryService>();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<ICartService, CartService>();
            serviceCollection.AddScoped<ICartItemService, CartItemService>();
            serviceCollection.AddScoped<ISizeService, SizeService>();
            serviceCollection.AddScoped<ISitemapService, SitemapService>();
            serviceCollection.AddScoped<IPageStringsService, PageStringsService>();
            serviceCollection.AddScoped<IProductImageService, ProductImageService>();
            serviceCollection.AddScoped<ICityDbService, CityDbService>();
            serviceCollection.AddScoped<IEmailService, EmailService>();
            serviceCollection.AddScoped<IGeneralService, GeneralService>();
            serviceCollection.AddScoped<IOrderService, OrderService>();
            serviceCollection.AddScoped<IFilterService, FilterService>();
            serviceCollection.AddScoped<IShippingService, ShippingService>();
            serviceCollection.AddScoped<IDiscountService, DiscountService>();
        }
    }
}
