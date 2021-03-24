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
                if (item.FullName == null || item.FullName.Length < 3 || item.FullName.Length > 20)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (item.Nickname == null && !item.Nickname.StartsWith("The "))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                char a = item.Nickname[4];
                char b = char.ToUpper(item.Nickname[4]);
                if (item.Nickname[4] != char.ToUpper(item.Nickname[4]))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (item.Age < 18 || item.Age > 65)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (item.Bail < 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                var mail = item.Mail.ToArray();
                int count = mail.Count(x => x.Address.EndsWith("str."));
                if (count != mail.Count())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                
                var z = item.IncarcerationDate;
                var currentPrisoner = new Prisoner
                {
                    Age = item.Age,
                    Bail = item.Bail ?? 0,
                    CellId = item.CellId,
                    FullName = item.FullName,
                    IncarcerationDate = DateTime.ParseExact(item.IncarcerationDate,"dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Nickname = item.Nickname,
                    ReleaseDate = DateTime.ParseExact(item.ReleaseDate,"dd/MM/yyyy", CultureInfo.InvariantCulture)
                };

                foreach (var Mail in item.Mail)
                {
                    var currentMail = new Mail
                    {
                        Address = Mail.Address,
                        Description = Mail.Description,
                        Sender = Mail.Sender
                    };
                    currentPrisoner.Mails.Add(currentMail);
                }
                context.Add(currentPrisoner);
                sb.AppendLine($"Imported {currentPrisoner.FullName} {currentPrisoner.Age} years old");
            }

            context.SaveChanges();
            return sb.ToString().TrimEnd();           
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