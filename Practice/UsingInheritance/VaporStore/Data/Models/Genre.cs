namespace VaporStore.Data.Models
{
    using System.Collections.Generic;

    public class Genre : Base
    {
        public Genre()
        {
            this.Games = new HashSet<Game>();
        }

        public ICollection<Game> Games { get; set; }
    }
}