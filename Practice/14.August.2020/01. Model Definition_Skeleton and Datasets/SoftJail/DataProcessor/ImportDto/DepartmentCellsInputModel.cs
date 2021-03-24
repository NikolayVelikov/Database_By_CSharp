using System.Collections.Generic;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentCellsInputModel
    {
        public DepartmentCellsInputModel()
        {
            this.Cells = new List<CellInputModel>();
        }

        public string Name { get; set; }
        public ICollection<CellInputModel> Cells { get; set; }
    }

    public class CellInputModel
    {
        public int CellNumber { get; set; }
        public bool HasWindow { get; set; }
    }
}