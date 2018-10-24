using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data
{
    public class StartUp
    {
        public static void Main()
        {
            using (FootballBettingContext context = new FootballBettingContext())
            {
                context.Database.EnsureDeleted();

                context.Database.EnsureCreated();
            }
        }
    }
}
