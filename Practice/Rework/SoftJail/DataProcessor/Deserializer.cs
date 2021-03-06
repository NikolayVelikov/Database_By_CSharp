﻿namespace SoftJail.DataProcessor
{

    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    using Data;
    using SoftJail.DataProcessor.ImportDto;
    using SoftJail.Data.Models;
    using System.Globalization;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentsJson = JsonConvert.DeserializeObject<DerparmentInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Department> departments = new List<Department>();
            foreach (var department in departmentsJson)
            {
                if (!IsValid(department) || !department.Cells.All(IsValid) || department.Cells.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentDepartment = new Department()
                {
                    Name = department.Name
                };
                foreach (var cell in department.Cells)
                {
                    var currentCell = new Cell()
                    {
                        CellNumber = cell.CellNumber,
                        HasWindow = cell.HasWindow
                    };
                    currentDepartment.Cells.Add(currentCell);
                }

                departments.Add(currentDepartment);
                sb.AppendLine($"Imported {currentDepartment.Name} with {currentDepartment.Cells.Count} cells");
            }
            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonerJson = JsonConvert.DeserializeObject<PrisonerInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Prisoner> prisoners = new List<Prisoner>();
            foreach (var prisoner in prisonerJson)
            {
                if (!IsValid(prisoner) || !prisoner.Mails.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime releaseDate;
                bool releaseDateFilled = DateTime.TryParseExact(prisoner.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                var currentPrisoner = new Prisoner()
                {
                    FullName = prisoner.FullName,
                    Nickname = prisoner.Nickname,
                    Age = prisoner.Age,
                    IncarcerationDate = DateTime.ParseExact(prisoner.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = releaseDate,
                    Bail = prisoner.Bail,
                    CellId = prisoner.CellId
                };

                foreach (var mail in prisoner.Mails)
                {
                    var currentMail = new Mail()
                    {
                        Description = mail.Description,
                        Sender = mail.Sender,
                        Address = mail.Address
                    };

                    currentPrisoner.Mails.Add(currentMail);
                }

                prisoners.Add(currentPrisoner);
                sb.AppendLine($"Imported {currentPrisoner.FullName} {currentPrisoner.Age} years old");
            }
            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            string root = "Officers";
            var officersXml = XmlConverter.Deserializer<OfficerInputModel>(xmlString, root);

            StringBuilder sb = new StringBuilder();
            List<Officer> officers = new List<Officer>();
            foreach (var officer in officersXml)
            {
                Position positon;
                Weapon weapon;
                if (!IsValid(officer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!Enum.TryParse<Position>(officer.Position, out positon))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!Enum.TryParse<Weapon>(officer.Weapon, out weapon))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Officer currentOfficer = new Officer()
                {
                    FullName = officer.FullName,
                    Salary = officer.Salary,
                    Position = positon,
                    Weapon = weapon,
                    DepartmentId = officer.DepartmentId      
                };

                foreach (var prisoner in officer.PrisionerId)
                {
                    var officerPrisoner = new OfficerPrisoner() { OfficerId = currentOfficer.Id, PrisonerId = prisoner.Id };
                    currentOfficer.OfficerPrisoners.Add(officerPrisoner);
                }

                officers.Add(currentOfficer);
                sb.AppendLine($"Imported {currentOfficer.FullName} ({currentOfficer.OfficerPrisoners.Count} prisoners)");
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