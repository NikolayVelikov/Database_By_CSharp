namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Linq;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                        .Where(p => ids.Contains(p.Id))
                        .Select(p => new
                        {
                            Id = p.Id,
                            Name = p.FullName,
                            CellNumber = p.Cell.CellNumber,
                            Officers = p.PrisonerOfficers.Select(of => new
                            {
                                OfficerName = of.Officer.FullName,
                                Department = of.Officer.Department.Name
                            }).OrderBy(of => of.OfficerName).ToList(),

                            TotalOfficerSalary = decimal.Parse(p.PrisonerOfficers.Sum(of => of.Officer.Salary).ToString("F2"))
                        }).OrderBy(p => p.Name).ThenBy(p => p.Id).ToList();

            var prisonerJson = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return prisonerJson;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string root = "Prisoners";
            string[] prisoners = prisonersNames.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray();

            var data = context.Prisoners
                .Where(x => prisoners.Contains(x.FullName))
                .Select(x => new PrisonerOutputModel()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd"),
                    EncryptedMessages = x.Mails.Select(x => new EncryptedMessages
                    {
                        Description = string.Join("",x.Description.Reverse())
                    }).ToArray()
                })
                .OrderBy(x=> x.FullName).ThenBy(x=> x.Id).ToList();

            var dataXml = XmlConverter.Serialize(data, root);

            return dataXml;
        }
    }
}