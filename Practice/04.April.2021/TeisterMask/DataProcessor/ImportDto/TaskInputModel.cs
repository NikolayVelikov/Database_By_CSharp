using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class TaskInputModel
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        [Required]
        public string DueDate { get; set; }

        [XmlElement("ExecutionType")]
        public string ExecutionType { get; set; }

        [XmlElement("LabelType")]
        public string LabelType { get; set; }
    }
}
