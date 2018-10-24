using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P03_FootballBetting.Data.Models.Configuration
{
    public class TeamConfig : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder
                .HasMany(x => x.AwayGames)
                .WithOne(c => c.AwayTeam)
                .HasForeignKey(fk => fk.GameId);

            builder
                .HasMany(x => x.HomeGames)
                .WithOne(c => c.HomeTeam)
                .HasForeignKey(fk => fk.HomeTeamId);

            builder
                .HasMany(x => x.Players)
                .WithOne(c => c.Team)
                .HasForeignKey(fk => fk.TeamId);
        }
    }
}
