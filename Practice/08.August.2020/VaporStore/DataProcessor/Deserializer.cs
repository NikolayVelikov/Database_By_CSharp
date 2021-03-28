namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using Data;
    using VaporStore.DataProcessor.Dto.Import;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var games = JsonConvert.DeserializeObject<GameImportModel[]>(jsonString);
            StringBuilder sb = new StringBuilder();

            foreach (var currentGame in games)
            {
                if (!IsValid(currentGame) || !currentGame.Tags.All(IsValid) || currentGame.Tags.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Developer dev = context.Developers.FirstOrDefault(x => x.Name == currentGame.Developer);
                Genre genre = context.Genres.FirstOrDefault(x => x.Name == currentGame.Genre);
                var tags = context.Tags.ToArray();

                if (dev == null)
                {
                    dev = new Developer() { Name = currentGame.Developer };
                    context.Developers.Add(dev);
                }
                if (genre == null)
                {
                    genre = new Genre() { Name = currentGame.Genre };
                    context.Genres.Add(genre);
                }

                foreach (var tag in currentGame.Tags)
                {
                    if (tags.FirstOrDefault(t => t.Name == tag) == null)
                    {
                        var t = new Tag() { Name = tag };
                        context.Tags.Add(t);
                    }
                }

                context.SaveChanges();

                var game = new Game()
                {
                    Name = currentGame.Name,
                    Price = currentGame.Price,
                    ReleaseDate = DateTime.ParseExact(currentGame.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Developer = dev,
                    Genre = genre,
                };
                context.Games.Add(game);

                context.SaveChanges();

                int gameId = game.Id;//context.Games.Where(x => x.Name == game.Name).Select(x => x.Id).FirstOrDefault();
                var tagsId = context.Tags.Where(t => currentGame.Tags.Contains(t.Name)).Select(t => t.Id).ToArray();

                foreach (var tagId in tagsId)
                {
                    var currentGameTag = new GameTag() { GameId = gameId, TagId = tagId };
                    context.GameTags.Add(currentGameTag);
                }
                context.SaveChanges();

                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {tagsId.Length} tags");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersJson = JsonConvert.DeserializeObject<UsersInputModel[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<User> users = new List<User>();
            foreach (var currentUser in usersJson)
            {
                bool valid = IsValid(currentUser);
                int nameLength = currentUser.Username.Length;
                string fulName = currentUser.FullName;

                if (!IsValid(currentUser))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (currentUser.Cards.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User()
                {
                    FullName = currentUser.FullName,
                    Username = currentUser.Username,
                    Age = currentUser.Age,
                    Email = currentUser.Email
                };

                foreach (var currentCard in currentUser.Cards)
                {
                    string cardType = currentCard.Type;
                    CardType type;
                    if (!Enum.TryParse<CardType>(cardType, out type) || !IsValid(currentCard))
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    var card = new Card()
                    {
                        Number = currentCard.Number,
                        Cvc = currentCard.CVC,
                        Type = type
                    };

                    user.Cards.Add(card);
                }

                users.Add(user);
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
            foreach (var currentPurchase in purchasesXml)
            {
                PurchaseType type;
                if (!IsValid(currentPurchase) || !Enum.TryParse<PurchaseType>(currentPurchase.Type, out type))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Game game = context.Games.FirstOrDefault(x => x.Name == currentPurchase.GameName);
                Card card = context.Cards.FirstOrDefault(x => x.Number == currentPurchase.CardNumber);
                DateTime date = DateTime.ParseExact(currentPurchase.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var purchase = new Purchase()
                {
                    Game = game,
                    Card = card,
                    Date = date,
                    ProductKey = currentPurchase.ProductKey,
                    Type = type
                };

                purchases.Add(purchase);
                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
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