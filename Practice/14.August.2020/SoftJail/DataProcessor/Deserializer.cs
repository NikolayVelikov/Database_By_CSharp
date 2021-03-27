namespace SoftJail.DataProcessor
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    using Newtonsoft.Json;

    using Data;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentsCells = JsonConvert.DeserializeObject<DepartmentCellImportModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Department> departments = new List<Department>();
            foreach (var currentDepartment in departmentsCells)
            {
                if (!IsValid(currentDepartment) || !currentDepartment.Cells.All(IsValid) || currentDepartment.Cells.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var department = new Department()
                {
                    Name = currentDepartment.Name,
                    Cells = currentDepartment.Cells.Select(x => new Cell
                    {
                        CellNumber = x.CellNumber,
                        HasWindow = x.HasWindow
                    }).ToList()
                };

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersMails = JsonConvert.DeserializeObject<PrisonersMailsInputModel[]>(jsonString);
            StringBuilder sb = new StringBuilder();

            List<Prisoner> prisoners = new List<Prisoner>();
            foreach (var currentPrisoner in prisonersMails)
            {
                if (!IsValid(currentPrisoner))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                else if (!currentPrisoner.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var incarcerationDate = DateTime.ParseExact(currentPrisoner.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var releaseDateValidation = DateTime.TryParseExact(
                    currentPrisoner.ReleaseDate,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime released);

                var prisoner = new Prisoner()
                {
                    FullName = currentPrisoner.FullName,
                    Nickname = currentPrisoner.Nickname,
                    Age = currentPrisoner.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDateValidation ? (DateTime?)released : null,
                    Bail = currentPrisoner.Bail,
                    CellId = currentPrisoner.CellId,
                    Mails = currentPrisoner.Mails.Select(x => new Mail
                    {
                        Address = x.Address,
                        Description = x.Description,
                        Sender = x.Sender
                    }).ToList()
                };

                prisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            string root = "Officers";
            var officersPrisoners = XmlConverter.Deserializer<OfficerPrisonerInputModel>(xmlString, root);

            List<Officer> officers = new List<Officer>();
            StringBuilder sb = new StringBuilder();
            foreach (var currentOfficer in officersPrisoners)
            {
                if (!IsValid(currentOfficer))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var officer = new Officer()
                {
                    FullName = currentOfficer.FullName,
                    Salary = currentOfficer.Salary,
                    Position = Enum.Parse<Position>(currentOfficer.Positon),
                    Weapon = Enum.Parse<Weapon>(currentOfficer.Weapon),
                    DepartmentId = currentOfficer.DepartmentId,
                    OfficerPrisoners = currentOfficer.Prisoners.Select(x => new OfficerPrisoner { PrisonerId = x.Id }).ToList()
                };

                officers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
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