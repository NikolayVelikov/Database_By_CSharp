using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.Output
{
    [XmlType("part")]
    public class PartsListOutputModel
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}