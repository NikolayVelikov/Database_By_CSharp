using SoftUni.Data;
using System.Linq;

namespace SoftUni.Solutions
{
    public class RemovesTown
    {
        public static string Solution(SoftUniContext context)
        {
            var townSeattle = context.Towns.FirstOrDefault(x => x.Name == "Seattle");
            var adresses = context.Addresses.Where(x => x.Town == townSeattle).ToList();
            var employees = context.Employees.Where(x => adresses.Contains(x.Address));

            context.Employees.RemoveRange(employees);
            context.Addresses.RemoveRange(adresses);
            context.Towns.Remove(townSeattle);

            context.SaveChanges();

            int count = adresses.Count;
            return $"{count} addresses in Seattle were deleted";
        }
    }
}
