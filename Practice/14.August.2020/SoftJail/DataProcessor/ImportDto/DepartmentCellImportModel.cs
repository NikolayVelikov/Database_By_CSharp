﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentCellImportModel
    {
        public DepartmentCellImportModel()
        {
            this.Cells = new List<CellInputModel>();
        }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<CellInputModel> Cells { get; set; }
    }

    public class CellInputModel
    {
        [Range(1,1000)]
        public int CellNumber { get; set; }

        public bool HasWindow { get; set; }
    }
}