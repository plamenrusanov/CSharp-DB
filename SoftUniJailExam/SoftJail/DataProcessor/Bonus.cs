namespace SoftJail.DataProcessor
{

    using Data;
    using SoftJail.Data.Models;
    using System;
    using System.Linq;

    public class Bonus
    {
        public static string ReleasePrisoner(SoftJailDbContext context, int prisonerId)
        {
            Prisoner prisoner = context.Prisoners.FirstOrDefault(x => x.Id == prisonerId);

            if (prisoner.ReleaseDate != null)
            {
                prisoner.ReleaseDate = DateTime.Now;
                prisoner.CellId = null;
                return $"Prisoner {prisoner.FullName} released";
            }

            return $"Prisoner {prisoner.FullName} is sentenced to life";
        }
    }
}
