using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P03_SalesDatabase.Data.Models.Configuration
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.CustomerId);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsUnicode();

            builder.Property(x => x.Email)
                .HasMaxLength(80)
                .IsUnicode(false);

        }
    }
}
