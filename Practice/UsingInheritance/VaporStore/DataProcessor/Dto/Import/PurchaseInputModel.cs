namespace VaporStore.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class PurchaseInputModel
    {
        [Required]
        [XmlAttribute("title")]
        public string GameName { get; set; }

        [Required]
        [XmlElement("Type")]
        public string PurchaseType { get; set; }

        [Required]
        [XmlElement("Key")]
        public string ProductionKey { get; set; }

        [Required]
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }
    }
}
//  <Purchase title="Dungeon Warfare 2">
//    <Type>Digital</Type>
//    <Key>ZTZ3-0D2S-G4TJ</Key>
//    <Card>1833 5024 0553 6211</Card>
//    <Date>07/12/2016 05:49</Date>
//  </Purchase>