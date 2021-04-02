namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    using Data;
    using VaporStore.DataProcessor.Dto.Import;
    using System.Text;
    using VaporStore.Data.Models;
    using System.Linq;
    using System.Globalization;
    using VaporStore.Data.Models.Enums;

    public static class Deserializer
    {
        private const string errorMessage = "Invalid Data";
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesJson = JsonConvert.DeserializeObject<GameDeveloperGenreTagInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Game> games = new List<Game>();
            foreach (var game in gamesJson)
            {
                if (!IsValid(game) || game.Tags.Length == 0)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
                var tags = games.SelectMany(p => p.GameTags).ToList();
                var developers = games.Select(x => x.Developer).ToList();
                var genres = games.Select(x => x.Genre).ToList();

                Developer developer = developers.FirstOrDefault(d => d.Name == game.Developer);
                if (developer == null)
                {
                    developer = new Developer() { Name = game.Developer };
                }

                Genre genre = genres.FirstOrDefault(g => g.Name == game.Genre);
                if (genre == null)
                {
                    genre = new Genre() { Name = game.Genre };
                }

                DateTime releaseDate = DateTime.ParseExact(game.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var currentGame = new Game()
                {
                    Name = game.Name,
                    Price = game.Price,
                    ReleaseDate = releaseDate,
                    Developer = developer,
                    Genre = genre,
                };

                foreach (var currentTag in game.Tags)
                {
                    if (currentTag == null)
                    {
                        continue;
                    }
                    GameTag gameTag = tags.FirstOrDefault(x => x.Tag.Name == currentTag);
                    if (gameTag == null)
                    {
                        gameTag = new GameTag() { Game = currentGame, Tag = new Tag() { Name = currentTag } };
                    }

                    currentGame.GameTags.Add(gameTag);
                }

                if (currentGame.GameTags.Count == 0)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                games.Add(currentGame);
                sb.AppendLine($"Added {currentGame.Name} ({currentGame.Genre.Name}) with {currentGame.GameTags.Count} tags");
            }
            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersJson = JsonConvert.DeserializeObject<UserAndCardInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<User> users = new List<User>();
            foreach (var user in usersJson)
            {
                if (!IsValid(user) || user.Cards.Count == 0)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                User currentUser = new User()
                {
                    FullName = user.FullName,
                    Username = user.Username,
                    Email = user.Email,
                    Age = user.Age,
                };

                foreach (var item in user.Cards)
                {
                    CardType type;

                    if (!Enum.TryParse<CardType>(item.Type, out type) || !IsValid(item))
                    {
                        sb.AppendLine(errorMessage);
                        continue;
                    }

                    Card card = new Card()
                    {
                        Cvc = item.CVC,
                        Number = item.Number,
                        Type = type
                    };

                    currentUser.Cards.Add(card);
                }

                users.Add(currentUser);
                sb.AppendLine($"Imported {currentUser.Username} with {currentUser.Cards.Count} cards");
            }
            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            string root = "Purchases";
            var purchasesXml = XmlConverter.Deserializer<PurchaseInputModel>(xmlString, root);

            StringBuilder sb = new StringBuilder();
            List<Purchase> purchases = new List<Purchase>();
            var cards = context.Cards.ToArray();
            var games = context.Games.ToArray();
            foreach (var purchase in purchasesXml)
            {
                PurchaseType type;
                if (!IsValid(purchase) || !Enum.TryParse<PurchaseType>(purchase.PurchaseType, out type))
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                DateTime date = DateTime.ParseExact(purchase.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var card = cards.FirstOrDefault(x => x.Number == purchase.CardNumber);
                var game = games.FirstOrDefault(x => x.Name == purchase.GameName);
                var currentPurchase = new Purchase()
                {
                    Card = card,
                    Date = date,
                    Game = game,
                    ProductKey = purchase.ProductionKey,
                    Type = type
                };

                purchases.Add(currentPurchase);
                sb.AppendLine($"Imported {currentPurchase.Game.Name} for {currentPurchase.Card.User.Username}");
            }
            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}