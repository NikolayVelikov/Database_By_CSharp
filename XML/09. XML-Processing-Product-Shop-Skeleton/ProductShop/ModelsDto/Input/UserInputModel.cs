using System.Xml.Serialization;

namespace ProductShop.ModelsDto.Input
{
    [XmlType("User")]
    public class UserInputModel
    {
        [XmlElement("firstName")]
        public string FristName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }
    }
}