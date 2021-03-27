using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

using SoftJail.Data.Models.Enums;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class OfficerPrisonerInputModel
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string FullName { get; set; }

        [XmlElement("Money")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Salary { get; set; }

        [XmlElement("Position")]
        [Required]
        [EnumDataType(typeof(Position))]
        public string Positon { get; set; }

        [XmlElement("Weapon")]
        [Required]
        [EnumDataType(typeof(Weapon))]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public PrisonersInputModel[] Prisoners { get; set; }
    }

    [XmlType("Prisoner")]
    public class PrisonersInputModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}