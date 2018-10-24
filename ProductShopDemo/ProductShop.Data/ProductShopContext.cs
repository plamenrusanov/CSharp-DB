using Microsoft.EntityFrameworkCore;
using ProductShop.Models;
using System;

namespace ProductShop.Data
{
    public class ProductShopContext : DbContext
    {
        public ProductShopContext()
        {

        }
        public ProductShopContext(DbContextOptions options) : base(options)
        {
        
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-D1G8VKM\\SQLEXPRESS;Database=ProductShop;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryProduct>().HasKey(x => new { x.ProductId, x.CategoryId });

            modelBuilder.Entity<User>(e =>
            {
                e.HasMany(x => x.ProductsSold)
                 .WithOne(s => s.Sellar)
                 .HasForeignKey(fk => fk.SellarId);

                e.HasMany(x => x.ProductsBought)
                 .WithOne(s => s.Buyer)
                 .HasForeignKey(fk => fk.BuyerId);
            });
        }

    }
}
