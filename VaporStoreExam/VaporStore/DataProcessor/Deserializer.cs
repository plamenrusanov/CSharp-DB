namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
      
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
            var deserializedGame = JsonConvert.DeserializeObject<GameDto[]>(jsonString);
            List<Game> games = new List<Game>();
            StringBuilder sb = new StringBuilder();

            foreach (var gameDto in deserializedGame)
            {
                if ( !IsValid(gameDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime date = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Genre genre = GetGenre(context, gameDto.Genre);

                if (!games.Any(x => x.Genre.Name == gameDto.Genre))
                {
                    genre = GetGenre(context, gameDto.Genre);
                }
                else
                {
                    genre = games.FirstOrDefault(x => x.Genre.Name == gameDto.Genre).Genre;
                }

                Developer developer = GetDeveloper(context, gameDto.Developer, games);

               

                if (gameDto.Tags == null || gameDto.Tags.Count < 1)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                List<Tag> tags = GetTag(context, gameDto.Tags, games);

                Game game = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    Developer = developer,
                    Genre = genre,
                    ReleaseDate = date,
                };

                foreach (var item in tags)
                {
                    game.GameTags.Add(new GameTag()
                    {
                        Tag = item
                    });
                }

                games.Add(game);
                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {tags.Count} tags");
            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static List<Tag> GetTag(VaporStoreDbContext context, List<string> gameTags, List<Game> games)
        {
            List<Tag> tags = new List<Tag>();

            foreach (var stringTag in gameTags)
            {
                Tag tag = context.Tags.FirstOrDefault(x => x.Name == stringTag);
                
                if (tag == null)
                {
                    var game =  games.Where(x => x.GameTags.Any(c => c.Tag.Name == stringTag)).FirstOrDefault();
                    if (game != null)
                    {
                        tag = game.GameTags.Select(x => x.Tag).FirstOrDefault(c => c.Name == stringTag);
                    }


                    if (tag == null)
                    {
                        tag = new Tag()
                        {
                            Name = stringTag
                        };
                    }
                }
                tags.Add(tag);
            }
            return tags;
        }

        private static Developer GetDeveloper(VaporStoreDbContext context, string developer, List<Game> games)
        {
            Developer dev = context.Developers.FirstOrDefault(x => x.Name == developer);

            if (dev == null)
            {
                dev = games.FirstOrDefault(x => x.Developer.Name == developer)?.Developer;

                if (dev == null)
                {
                    dev = new Developer()
                    {
                        Name = developer,
                    };
                }
               
            }

            return dev;

        }

        private static Genre GetGenre(VaporStoreDbContext context, string genre)
        {
            var gen = context.Genres.FirstOrDefault(x => x.Name == genre);

            if (gen == null)
            {
                gen = new Genre()
                {
                    Name = genre,
                };
            }

            
            return gen;
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
            var deserializedUsers = JsonConvert.DeserializeObject<UserDto[]>(jsonString);
            List<User> users = new List<User>();
            StringBuilder sb = new StringBuilder();

            foreach (var userDto in deserializedUsers)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                List<Card> cards = new List<Card>();
                bool isValidCards = true;
                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        isValidCards = false;
                        break;
                    }

                    CardType cardType;
                    bool isValidType = Enum.TryParse(cardDto.Type, out cardType);

                    if (!isValidType)
                    {
                        isValidCards = false;
                        break;
                    }

                    Card card = new Card()
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.Cvc,
                        Type = cardType
                    } ;

                    cards.Add(card);

                }

                if (!isValidCards)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                User user = new User()
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age,
                    Cards = cards
                };

                users.Add(user);
                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
            StringBuilder sb = new StringBuilder();
            List<Purchase> purchases = new List<Purchase>();

            var deserializer = new XmlSerializer(typeof(PurchaseDto[]), new XmlRootAttribute("Purchases"));

            var deserializedPurchases = (PurchaseDto[])deserializer.Deserialize(new StringReader(xmlString));

            foreach (var purchaseDto in deserializedPurchases)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                PurchaseType purchaseType;
                bool isValidPurchaseType = Enum.TryParse(purchaseDto.Type, out purchaseType);
                if (!isValidPurchaseType)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime dateTime = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                Card card = context.Cards.FirstOrDefault(x => x.Number == purchaseDto.Card);

                Game game = context.Games.FirstOrDefault(x => x.Name == purchaseDto.Title);

                if (card == null || game == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Purchase purchase = new Purchase()
                {
                    Type = purchaseType,
                    Date = dateTime,
                    Card = card,
                    Game = game,
                    ProductKey = purchaseDto.Key
                };

                purchases.Add(purchase);
                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
            }
            context.Purchases.AddRange(purchases);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}