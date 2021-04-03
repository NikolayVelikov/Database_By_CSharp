namespace SoftJail.DataProcessor.ImportDto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DerparmentInputModel
    {
        public DerparmentInputModel()
        {
            this.Cells = new List<CellInputModel>();
        }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }
        public ICollection<CellInputModel> Cells { get; set; }
        public bool All { get; internal set; }
    }
}
