namespace SoftJail.DataProcessor.ImportDto
{
    using System.Xml.Serialization;
    using System.ComponentModel.DataAnnotations;

    [XmlType("Officer")]
    public class OfficerInputModel
    {
        [Required]
        [XmlElement("Name")]
        [StringLength(30, MinimumLength = 3)]
        public string FullName { get; set; }

        [XmlElement("Money")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Salary { get; set; }

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }

        [Required]
        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]        
        public Prisoners[] PrisionerId { get; set; }
    }

    [XmlType("Prisoner")]
    public class Prisoners
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
//<Officers>
//	<Officer>
//		<Name>Minerva Kitchingman</Name>
//		<Money>2582</Money>
//		<Position>Invalid</Position>
//		<Weapon>ChainRifle</Weapon>
//		<DepartmentId>2</DepartmentId>
//		<Prisoners>
//			<Prisoner id="15" />
//		</Prisoners>
//	</Officer>