using System.Xml.Serialization;

namespace ProductShop.ModelsDto.Input
{
    [XmlType("Category")]
    public class CategoriesInputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}