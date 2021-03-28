namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var game = context.Genres
                    .Where(g => genreNames.Contains(g.Name) && g.Games.Select(g => g.Purchases.Any()).Count() != 0)
                    .Select(g => new
                    {
                        Id = g.Id,
                        Genre = g.Name,
                        Games = g.Games
                        .Select(g => new
                        {
                            Id = g.Id,
                            Title = g.Name,
                            Developer = g.Developer.Name,
                            Tags = g.GameTags.Select(t => t.Tag.Name).ToList(),
                            Players = g.Purchases.Count
                        }).OrderByDescending(x => x.Players).ThenBy(x => x.Id).ToList(),
                        TotalPlayers = g.Games.Select(x => new { number = x.Purchases.Count() }).Count()
                    }).OrderByDescending(x => x.TotalPlayers).ThenBy(x => x.Id).ToList();

            var gamesJson = JsonConvert.SerializeObject(game, Formatting.Indented);

            return gamesJson;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            throw new NotImplementedException();
        }
    }
}