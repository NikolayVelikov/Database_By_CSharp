namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using SoftJail.XmlConvert;
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
                if (department == null || department.Length < 3 || department.Length > 25)
                {
                    sb.AppendLine("Invalid data");
                    continue;
                }
                if (cells.Length == 0)
                {
                    sb.AppendLine("Invalid data");
                    continue;
                }
                if (cells.FirstOrDefault(x=>x.CellNumber == null) != null || cells.FirstOrDefault(x => x.CellNumber < 1 || x.CellNumber > 1000) != null)
                {
                    sb.AppendLine("Invalid data");
                    continue;
                }
                if (cells.FirstOrDefault(x=> x.HasWindow == null) != null)
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
            List<Prisoner> prisioners = new List<Prisoner>();

            foreach (var item in jsongFile)
            {
                var jsonMail = item.Mails.ToList();
                if (item.FullName == null || item.FullName.Length < 3 || item.FullName.Length > 20)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (item.Nickname == null || !item.Nickname.StartsWith("The ") || !item.Nickname.All(n => char.IsLetter(n) || char.IsWhiteSpace(n)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                } // Possible problem with UpperCase
                if (item.Age == null || item.Age < 18 || item.Age > 65)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (item.IncarcerationDate == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (item.Bail < 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (jsonMail.FirstOrDefault(d => d.Description == null) != null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (jsonMail.FirstOrDefault(s => s.Sender == null) != null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (jsonMail.FirstOrDefault(a => a.Address == null) != null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (jsonMail.FirstOrDefault(a => !a.Address.EndsWith("str.")) != null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime? date = null;
                if (item.ReleaseDate != null)
                {
                    date = DateTime.ParseExact(item.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                Prisoner prisioner = new Prisoner
                {
                    FullName = item.FullName,
                    Age = (int)item.Age,
                    Bail = item.Bail,
                    CellId = item.CellId,
                    Nickname = item.Nickname,
                    IncarcerationDate = DateTime.ParseExact(item.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = date,
                    Mails = item.Mails
                    .Select(x => new Mail
                    {
                        Address = x.Address,
                        Description = x.Description,
                        Sender = x.Sender
                    }).ToList()
                };

                prisioners.Add(prisioner);
                sb.AppendLine($"Imported {prisioner.FullName} {prisioner.Age} years old");
            }

            context.AddRange(prisioners);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            string root = "Officers";
            var officersXml = XmlConverter.Deserializer<OfficerPrisionerInputModel>(xmlString, root);

            var departmentIds = context.Departments.Select(x => x.Id).ToList();
            var prisionersId = context.Prisoners.Select(x => x.Id).ToList();

            List<Officer> officers = new List<Officer>();
            StringBuilder sb = new StringBuilder();
            foreach (var officer in officersXml)
            {
                var currentPrisioners = officer.Prisoners.Select(x => int.Parse(x.Id));
                if (officer.FullName == null || officer.FullName.Length < 3 || officer.FullName.Length > 30)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (officer.Salary == null || officer.Salary < 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                Position position;
                if (officer.Position == null || !Enum.TryParse<Position>(officer.Position, out position))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                Weapon weapon;
                if (officer.Weapon == null || !Enum.TryParse<Weapon>(officer.Weapon, out weapon))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (!departmentIds.Contains(officer.DepartmentId))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer currentOfficer = new Officer
                {
                    FullName = officer.FullName,
                    Salary = officer.Salary,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = officer.DepartmentId,
                };

                foreach (var id in currentPrisioners)
                {
                    var prisionerId = prisionersId.FirstOrDefault(Id => Id == id);
                    var officerPrisioner = new OfficerPrisoner() { OfficerId = id, PrisonerId = prisionerId };
                    currentOfficer.OfficerPrisoners.Add(officerPrisioner);
                }

                officers.Add(currentOfficer);
                sb.AppendLine($"Imported {currentOfficer.FullName} ({currentOfficer.OfficerPrisoners.Count()} prisoners)");
            }

            context.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
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