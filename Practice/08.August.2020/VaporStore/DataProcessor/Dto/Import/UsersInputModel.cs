using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class UsersInputModel
    {
        public UsersInputModel()
        {
            this.Cards = new List<CardsInputModel>();
        }

        [Required]
        [RegularExpression(@"[A-Z][a-z]+ [A-Z][a-z]+")]
        public string FullName { get; set; }

        [Required]        
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(3,103)]
        public int Age { get; set; }

        public ICollection<CardsInputModel> Cards { get; set; }
    }
    public class CardsInputModel
    {
        [Required]
        [RegularExpression(@"\d{4} \d{4} \d{4} \d{4}")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}")]
        public string CVC { get; set; }

        [Required]
        public string Type { get; set; }
    }
}