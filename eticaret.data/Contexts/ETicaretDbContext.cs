using eticaret.entity.Identity;
using eticaret.entity.Offer;
using eticaret.entity.Order;
using eticaret.entity.PageEntities;
using eticaret.entity.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Contexts
{
    public class ETicaretDbContext: IdentityDbContext<AppUser, AppRole, string>
    {
        public DbSet<et.PageStrings.PageStrings> PageStrings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<et.Category.Category> Categories { get; set; }
        public DbSet<et.Category.CategoryImage> CategoryImages { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<et.Cart.Cart> Carts { get; set; }
        public DbSet<et.Cart.CartItem> CartItems { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<et.Filter.FilterBox> FilterBoxs { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<et.File> Files { get; set; }
        public DbSet<RelatedProduct> RelatedProducts { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<ProductQA> ProductQAs { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<ProductRate> ProductRates { get; set; }
        public DbSet<SupportFormModel> SupportMails { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<SupportMailResponseModel> SupportMailResponses { get; set; }
        public DbSet<et.Log.PageLog> PageLogs { get; set; }
        public DbSet<et.Cargo.Shipping> ShippingCompanies { get; set; }
        public ETicaretDbContext(DbContextOptions options)
            : base (options) {  }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductSize>()
                        .HasKey(ps => new { ps.ProductId, ps.SizeId });
            modelBuilder.Entity<ProductSize>()
                .HasOne(ps => ps.Product)
                .WithMany(b => b.ProductSizes)
                .HasForeignKey(bc => bc.ProductId);

            modelBuilder.Entity<ProductSize>()
                .HasOne(ps => ps.Product)
                .WithMany(b => b.ProductSizes)
                .HasForeignKey(bc => bc.ProductId);


            modelBuilder.Entity<ProductComment>()
                    .Property(u => u.Id).ValueGeneratedNever();
            modelBuilder.Entity<ProductRate>()
                    .Property(u => u.Id).ValueGeneratedNever();
        }
    }
}
