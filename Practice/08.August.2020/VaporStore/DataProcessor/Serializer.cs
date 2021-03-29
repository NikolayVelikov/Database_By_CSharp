namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var games = context.Genres.ToList()
                .Where(x => genreNames.Contains(x.Name))
                .Select(x => new
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games.Select(g => new
                    {
                        Id = g.Id,
                        Title = g.Name,
                        Developer = g.Developer.Name,
                        Tags = string.Join(", ", g.GameTags.Select(t => t.Tag.Name)),
                        Players = g.Purchases.Count()
                    })
                    .Where(g => g.Players > 0)
                    .OrderByDescending(p => p.Players)
                    .ThenBy(g => g.Id),
                    TotalPlayers = x.Games.Sum(g => g.Purchases.Count())
                }).OrderByDescending(x => x.TotalPlayers).ThenBy(x => x.Id);



            var gamesJson = JsonConvert.SerializeObject(games, Formatting.Indented);

            return gamesJson;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var data = context.Users.ToList()
                .Where(x => x.Cards.Any(c => c.Purchases.Any(p=> p.Type.ToString() == storeType)))
                .Select(x => new UserOutputModel()
                {
                    UserName = x.Username,
                    TotalSpent = x.Cards.Sum(c => c.Purchases.Where(p => p.Type.ToString() == storeType).Sum(p => p.Game.Price)),
                    Purchases = x.Cards.SelectMany(c => c.Purchases)
                    .Where(p=> p.Type.ToString() == storeType)
                    .Select(p => new PurchaseOutpuModel()
                    {
                        Cvc = p.Card.Cvc,
                        Card = p.Card.Number,
                        Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new GameOutputModel()
                        {
                            Genre = p.Game.Genre.Name,
                            Price = p.Game.Price,
                            Title = p.Game.Name
                        }
                    })
                    .OrderByDescending(x=> x.Date).ToArray()
                }).OrderByDescending(x => x.TotalSpent).ThenBy(x => x.UserName).ToArray();

            var dataXml = XmlConverter.Serialize<UserOutputModel>(data, "Users");

            return dataXml;
        }
    }
}