namespace BookShop.DataProcessor.ImportDto
{
    using System.Xml.Serialization;
    using System.ComponentModel.DataAnnotations;

    [XmlType("Book")]
    public class BookImportModel
    {
        [Required]
        [XmlElement("Name")]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [XmlElement("Genre")]
        [Range(1, 3)]
        public int Genre { get; set; }

        [XmlElement("Price")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [XmlElement("Pages")]
        [Range(50, 5000)]
        public int Pages { get; set; }

        [XmlElement("PublishedOn")]
        public string PublishedOn { get; set; }
    }
}
//<Book>
//    <Name>Hairy Torchwood</Name>
//    <Genre>3</Genre>
//    <Price>41.99</Price>
//    <Pages>3013</Pages>
//    <PublishedOn>01/13/2013</PublishedOn>
//  </Book>