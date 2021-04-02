namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public abstract class Base
    {     
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
