using System.Xml.Serialization;

namespace ProductShop.ModelsDto.Output
{
    [XmlType("Category")]
    public class CategoriesOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("count")]
        public int NumberOfProducts { get; set; }

        [XmlElement("averagePrice")]
        public decimal AveragePrice { get; set; }

        [XmlElement("totalRevenue")]
        public decimal TotalRevenue { get; set; }
    }
}