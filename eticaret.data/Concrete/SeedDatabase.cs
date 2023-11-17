using eticaret.data.Abstract.Order;
using eticaret.data.Concrete.Category;
using eticaret.data.Concrete.Product;
using eticaret.data.Contexts;
using eticaret.data.Services.Abstract;
using eticaret.entity.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Concrete
{
    public static class SeedDatabase
    {

        readonly static DesginTimeDbContextFactory designFactory = new();
        readonly static string[] args = { "3", "1" };
        readonly static ETicaretDbContext _context = designFactory.CreateDbContext(args);
        readonly static ProductRepository _productRepository 
            = new ProductRepository(_context);
        readonly static CategoryRepository _categoryRepository
            = new CategoryRepository(_context);

        readonly static SubCategoryRepository _subCategoryRepository
            = new SubCategoryRepository(_context);

        public async static Task Seed(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var citiesManager = scope.ServiceProvider.GetService<ICitiesDbService>();
            var rolesManager = scope.ServiceProvider.GetService<RoleManager<AppRole>>();
            var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
            var orderStatusRepo = scope.ServiceProvider.GetService<IOrderStatusRepository>();
            var context = scope.ServiceProvider.GetService<ETicaretDbContext>();
            int roleCount = rolesManager.Roles.Count();
            if (roleCount < 1)
            {
                await rolesManager.CreateAsync(new() { Name = "Admin" });
            }
            int userCount = userManager.Users.Count();
            if (userCount < 1)
            {
                AppUser user = new AppUser()
                {
                    UserName = "admin",
                    Email = "admin@hotmail.com",
                    FirstName = "admin",
                    LastName = "admin",
                    Id = Guid.NewGuid().ToString(),
                    EmailConfirmed = true
                };
                IdentityResult result = await userManager.CreateAsync(user, "Gokhan949..");
                await userManager.AddToRoleAsync(user, "Admin");
                await userManager.UpdateAsync(user);
            }
            if(orderStatusRepo.Table.Count() < 1)
            {
                string[] status = { "Onay Bekliyor", "Onaylandı", "Kargoya Verildi", "Tamamlandı", "İptal Bekliyor", "İptal Edildi", "İade Bekliyor", "İade Edildi", "Değişim Bekliyor", "Değiştirildi" };
                for (int i = 0; i < status.Count(); i++)
                {
                    orderStatusRepo.AddAsync(new()
                    {
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        Title = status[i]
                    });
                }
                await orderStatusRepo.SaveAsync();
            }

            if (context.ShippingCompanies.Count() < 1)
            {
                string[] Titles = { "MNG Kargo", "Aras Kargo", "PTT Kargo", "Yurtiçi Kargo", "Sürat Kargo" };
                for (int i = 0; i < Titles.Count(); i++)
                {
                    context.ShippingCompanies.Add(new et.Cargo.Shipping
                    {
                       Id = Guid.NewGuid(),
                       CreateDate = DateTime.Now,
                       UpdateDate = DateTime.Now,
                       Description = "",
                       Name = Titles[i],
                       Price = 0,
                       IsDefault = (i == 0) ? true : false
                    });
                }
                context.SaveChanges();
            }
        }

        public static async void Seed()
        {
            Console.WriteLine("SEEDING...");
            if (_productRepository.GetAll().Count() == 0)
            {
                Console.WriteLine("PRODUCTS SEEDING..");
                List<et.Product.Product> products = new()
            {
                new ()
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Name = "Urun 1",
                    Price = 10,
                    Stock = 15,
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Name = "Urun 2",
                    Price = 11,
                    Stock = 16,
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Name = "Urun 3",
                    Price = 12,
                    Stock = 17,
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Name = "Urun 4",
                    Price = 13,
                    Stock = 18,
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Name = "Urun 5",
                    Price = 14,
                    Stock = 19,
                },
            };
                await _productRepository.AddRangeAsync(products);
            }
            if (_categoryRepository.GetAll().Count() == 0) 
            {
                Console.WriteLine("CATEGORIES SEEDING..");
                List<et.Category.Category> categories = new()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Telefon",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Beyaz Eşya",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,

                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Bilgisayar",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                },
            };
                await _categoryRepository.AddRangeAsync(categories);
            }
            _productRepository?.SaveAsync();
        }
    }
}
