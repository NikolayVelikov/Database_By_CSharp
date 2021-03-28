namespace VaporStore.DataProcessor.Dto.Import
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class GameImportModel
    {
        public GameImportModel()
        {
            this.Tags = new List<string>();
        }

        [Required]
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        public string ReleaseDate { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public string Genre { get; set; }

        public ICollection<string> Tags { get; set; }

    }
}
//  "Name": "Tom Clancy's Rainbow Six Siege",
//  "Price": 14.99,
//  "ReleaseDate": "2015-12-01",
//  "Developer": "Ubisoft Montreal",
//  "Genre": "Action",
//  "Tags": [
//  	"Single-player",
//  	"Multi-player",
//  	"Co-op",
//  	"Steam Trading Cards",
//  	"In-App Purchases",
//  	"Partial Controller Support"
//  ] 