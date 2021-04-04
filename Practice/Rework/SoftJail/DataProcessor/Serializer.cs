namespace SoftJail.DataProcessor
{

    using Newtonsoft.Json;
    using Data;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Linq;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .ToList()
                .Select(p => new PrisonerOutputModel()
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                    .Select(of => new OfficerOutputModel()
                    {
                        OfficerName = of.Officer.FullName,
                        Department = of.Officer.Department.Name
                    }).OrderBy(of => of.OfficerName).ToList(),

                    TotalOfficerSalary = p.PrisonerOfficers.Sum(x => x.Officer.Salary)
                })
                .OrderBy(p => p.Name).ThenBy(p => p.Id).ToList();

            var prisonersJson = JsonConvert.SerializeObject(prisoners, Formatting.Indented);
            return prisonersJson;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            throw new NotImplementedException();
        }
    }
}