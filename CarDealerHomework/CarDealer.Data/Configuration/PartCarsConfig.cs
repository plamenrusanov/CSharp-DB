using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Data.Configuration
{
    public class PartCarsConfig : IEntityTypeConfiguration<PartCars>
    {
        public void Configure(EntityTypeBuilder<PartCars> builder)
        {
            builder.HasKey(x => new { x.CarId, x.PartId });      
        }
    }
}
