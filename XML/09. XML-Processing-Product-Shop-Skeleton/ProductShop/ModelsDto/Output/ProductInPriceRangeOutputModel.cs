using System.Xml.Serialization;

namespace ProductShop.ModelsDto.Output
{
    [XmlType("Product")]
    public class ProductInPriceRangeOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("buyer")]
        public string Buyer { get; set; }
    }
}
