namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class GameTag
    {
        [ForeignKey("Game")]
        public int GameId { get; set; }
        public Game Game { get; set; }

        [ForeignKey("Tag")]
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}

//•	GameId – integer, Primary Key, foreign key (required)
//•	Game – Game
//•	TagId – integer, Primary Key, foreign key (required)
//•	Tag – Tag