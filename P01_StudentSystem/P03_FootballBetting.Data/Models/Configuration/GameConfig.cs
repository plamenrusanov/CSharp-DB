using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P03_FootballBetting.Data.Models.Configuration
{
    public class GameConfig : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .HasMany(x => x.Bets)
                .WithOne(g => g.Game)
                .HasForeignKey(fk => fk.GameId);

            builder
                .HasMany(x => x.PlayerStatistics)
                .WithOne(c => c.Game)
                .HasForeignKey(fk => fk.GameId );
        }
    }
}
