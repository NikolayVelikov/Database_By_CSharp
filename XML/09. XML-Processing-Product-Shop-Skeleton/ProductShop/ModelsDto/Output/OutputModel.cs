using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.ModelsDto.Output
{
    [XmlType("Users")]
    public class OutputModel
    {
        public OutputModel()
        {
            this.Users = new List<UserOutputModel>();
        }

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public List<UserOutputModel> Users { get; set; }
    }
}
