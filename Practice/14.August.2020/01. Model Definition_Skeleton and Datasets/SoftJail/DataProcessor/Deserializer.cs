namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;    

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var jsongFile = JsonConvert.DeserializeObject<DepartmentCellsInputModel[]>(jsonString);
            foreach (var item in jsongFile)
            {
                var department = item.Name;
                var cells = item.Cells.ToArray();
                if (department.Length < 3 || department.Length > 25)
                {
                    sb.AppendLine("Invalid data");
                    continue;
                }
                if (cells.Length == 0)
                {
                    sb.AppendLine("Invalid data");
                    continue;
                }
                if (cells.FirstOrDefault(x => x.CellNumber < 1 || x.CellNumber > 1000) != null)
                {
                    sb.AppendLine("Invalid data");
                    continue;
                }

                Department currentDepartment = new Department
                {
                    Name = department,
                };
                foreach (var cell in cells)
                {
                    Cell currentCell = new Cell
                    {
                        CellNumber = cell.CellNumber,
                        HasWindow = cell.HasWindow
                    };

                    currentDepartment.Cells.Add(currentCell);
                }

                context.Add(currentDepartment);
                sb.AppendLine($"Imported {currentDepartment.Name} with {cells.Length} cells");
            }
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var jsongFile = JsonConvert.DeserializeObject<PrisonerMailsInputModel[]>(jsonString);

            foreach (var item in jsongFile)
            {
                var jsonMail = item.Mail.ToList();
                if (item.FullName.Length < 3 || item.FullName.Length > 20 || item.FullName.Length == null)
                {
                    sb.AppendLine("Invalid Data");
                }
                if (!item.Nickname.StartsWith("The ") || item.Nickname == null)
                {
                    sb.AppendLine("Invalid Data");
                } // Possible problem with UpperCase
                if (item.Age < 18 || item.Age > 65 || item.Age == null)
                {
                    sb.AppendLine("Invalid Data");
                }
                if (item.IncarcerationDate == null)
                {
                    sb.AppendLine("Invalid Data");
                }
                if (item.Bail < 0)
                {
                    sb.AppendLine("Invalid Data");
                }
                if (jsonMail.FirstOrDefault(d => d.Description == null) != null)
                {
                    sb.AppendLine("Invalid Data");
                }
                if (jsonMail.FirstOrDefault(s=> s.Sender == null) != null)
                {
                    sb.AppendLine("Invalid Data");
                }

            }

            return null;          
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}