namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
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
            throw new NotImplementedException();
        }
    }
}