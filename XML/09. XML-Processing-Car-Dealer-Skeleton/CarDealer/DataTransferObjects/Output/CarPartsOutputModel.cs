using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.Output
{
    [XmlType("car")]
    public class CarPartsOutputModel
    {
        public CarPartsOutputModel()
        {
            this.PartsListOutputModel = new List<PartsListOutputModel>();
        }

        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistanced { get; set; }

        [XmlArray("parts")]
        public List<PartsListOutputModel> PartsListOutputModel { get; set; }
    }
}
