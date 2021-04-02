namespace VaporStore.DataProcessor.Dto.Import
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserAndCardInputModel
    {
        [Required]
        [RegularExpression(@"^([A-Z][a-z]+ [A-Z][a-z]+)$")]
        public string FullName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(3, 103)]
        public int Age { get; set; }

        public ICollection<CardInput> Cards { get; set; }

        public class CardInput
        {
            [Required]
            [RegularExpression(@"^(\d{4} \d{4} \d{4} \d{4})$")]
            public string Number { get; set; }

            [Required]
            [RegularExpression(@"^(\d{3})$")]
            public string CVC { get; set; }
            public string Type { get; set; }
        }
    }
}
//  "FullName": "",
//  "Username": "invalid",
//  "Email": "invalid@invalid.com",
//  "Age": 20,
//  "Cards":
//  [
//    {
//      "Number": "1111 1111 1111 1111",
//      "CVC": "111",
//      "Type": "Debit"
//    }
//  ]