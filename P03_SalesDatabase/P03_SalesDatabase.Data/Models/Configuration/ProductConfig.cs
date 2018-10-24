using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P03_SalesDatabase.Data.Models.Configuration
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode();

            builder.Property(x => x.Description)
                .HasMaxLength(250)
                .HasDefaultValue("No description");
        }
    }
}
