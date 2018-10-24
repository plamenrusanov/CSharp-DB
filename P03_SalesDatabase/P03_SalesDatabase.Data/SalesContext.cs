using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using P03_SalesDatabase.Data.Models.Configuration;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Store> Stores { get; set; }
        DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-D1G8VKM\SQLEXPRESS;Database=Sales;Integrated Security=True");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new StoreConfig());
            modelBuilder.ApplyConfiguration(new SaleConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
