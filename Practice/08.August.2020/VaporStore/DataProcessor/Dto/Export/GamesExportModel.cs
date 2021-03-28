using System.Collections.Generic;

namespace VaporStore.DataProcessor.Dto.Export
{
    public class GamesExportModel
    {
        public GamesExportModel()
        {
            this.Games = new List<GameExportModel>();
        }

        public int Id { get; set; }
        public string Genre { get; set; }
        public ICollection<GameExportModel> Games { get; set; }

    }
    public class GameExportModel
    {
        public GameExportModel()
        {
            this.Tags = new List<TagExportModel>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Developer { get; set; }
        public ICollection<TagExportModel> Tags { get; set; }
        public int Players { get; set; }
    }
    public class TagExportModel
    {
        public string Tag { get; set; }
    }
}
//      Id = x.GenreId,
//      Genre = x.Genre.Name,
//      Games = new
//      {
//          Id = x.Id,
//          Title = x.Name,
//          Developer = x.Developer.Name,
//          Tags = x.GameTags.Select(t => t.Tag.Name).ToList(),
//          Players = x.Purchases.Count
//      }