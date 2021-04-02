using System.Collections.Generic;

namespace VaporStore.Data.Models
{
    public class Tag : Base
    {
        public Tag()
        {
            this.GameTags = new HashSet<GameTag>();
        }

        public ICollection<GameTag> GameTags;
    }
}
