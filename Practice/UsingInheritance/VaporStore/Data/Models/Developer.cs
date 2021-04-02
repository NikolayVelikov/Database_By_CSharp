namespace VaporStore.Data.Models
{
    using System.Collections.Generic;

    public class Developer : Base
    {
        public Developer()
        {
            this.Games = new HashSet<Game>();
        }

        public ICollection<Game> Games { get; set; }
    }
}