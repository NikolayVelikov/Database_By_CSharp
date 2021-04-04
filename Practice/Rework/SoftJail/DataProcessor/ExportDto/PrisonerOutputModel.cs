namespace SoftJail.DataProcessor.ExportDto
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class PrisonerOutputModel
    {
        public PrisonerOutputModel()
        {
            this.Officers = new List<OfficerOutputModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int CellNumber { get; set; }

        public ICollection<OfficerOutputModel> Officers { get; set; }

        public decimal TotalOfficerSalary { get; set; }
    }
}