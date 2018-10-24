using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P03_FootballBetting.Data.Models.Configuration
{
    public class ColorConfig : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder
                .HasMany(x => x.PrimaryKitTeams)
                .WithOne(c => c.PrimaryKitColor)
                .HasForeignKey(fk => fk.PrimaryKitColorId);

            builder
                .HasMany(x => x.SecondaryKitTeams)
                .WithOne(c => c.SecondaryKitColor)
                .HasForeignKey(fk => fk.SecondaryKitColorId);
        }
    }
}
