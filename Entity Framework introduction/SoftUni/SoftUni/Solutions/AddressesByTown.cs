using SoftUni.Data;
using System.Linq;
using System.Text;

namespace SoftUni.Solutions
{
    public class AddressesByTown
    {
        public static string Solution(SoftUniContext context)
        {
            var adresses = context.Addresses.Select(x => new
            {
                x.AddressText,
                x.Town.Name,
                x.Employees.Count
            }
            ).OrderByDescending(x => x.Count).ThenBy(x => x.Name).ThenBy(x => x.AddressText).Take(10).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var address in adresses)
            {
                string addressText = address.AddressText;
                string townName = address.Name;
                int employeeCount = address.Count;

                sb.AppendLine($"{addressText}, {townName} - {employeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
