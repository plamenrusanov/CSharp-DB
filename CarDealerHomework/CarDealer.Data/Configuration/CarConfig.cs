using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Data.Configuration
{
    public class CarConfig : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasMany(x => x.PartCars)
                .WithOne(c => c.Car)
                .HasForeignKey(fk => fk.CarId);

            builder.HasOne(x => x.Sale)
                .WithOne(c => c.Car)
                .HasForeignKey<Sale>(x => x.CarId);
                    
        }
    }
}
