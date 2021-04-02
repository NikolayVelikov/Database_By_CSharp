namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public abstract class Base
    {     
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
