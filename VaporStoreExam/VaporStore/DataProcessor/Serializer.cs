namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
            var games = context.Genres 
                .Where(x => genreNames.Any(c => c == x.Name))
                .Select(x => new
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games.Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Name,
                        Developer = c.Developer.Name,
                        Tags = string.Join(", ", c.GameTags.Select(v => v.Tag.Name)),
                        Players = c.Purchases.Count,
                    })
                    .Where(c => c.Players > 0)
                    .OrderByDescending(c => c.Players)
                    .ThenBy(c => c.Id)
                    .ToArray(),
                    TotalPlayers = x.Games.Sum(c => c.Purchases.Count)

                })
                .OrderByDescending(c => c.TotalPlayers)
                .ThenBy(c => c.Id)
                .ToArray();
                
            var jsonString = JsonConvert.SerializeObject(games, Newtonsoft.Json.Formatting.Indented);

            return jsonString;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType )
		{
            PurchaseType purchaseType;
            Enum.TryParse(storeType, out purchaseType);
  
            StringBuilder sb = new StringBuilder();
          
            var users = context.Users
                
                .Select(z => new UserDto()
                {
                    Username = z.Username,
                    Purchases = z.Cards.SelectMany(x => x.Purchases)
                    .Where(x => x.Type == purchaseType)
                    .Select(x => new PurchaseDto()
                    {
                        Card = x.Card.Number,
                        Cvc = x.Card.Cvc,
                        Date = x.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new GameDto()
                        {
                            Title = x.Game.Name,
                            Genre = x.Game.Genre.Name,
                            Price = x.Game.Price
                        },
                     
                    })
                
                    .OrderBy(x => x.Date).ToList(),
                   
                    TotalSpent = z.Cards.SelectMany(x => x.Purchases).Where(x => x.Type == purchaseType).Sum(c => c.Game.Price)

                })
                .Where(x => x.Purchases.Count != 0)
                .OrderByDescending(x => x.TotalSpent)
                .ThenBy(x => x.Username)
                .ToArray();

            var serializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("Users"));
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), users, xmlNamespaces);

            return sb.ToString().Trim();
        }

    }
}
