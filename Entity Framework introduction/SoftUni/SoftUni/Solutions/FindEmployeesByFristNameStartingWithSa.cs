using SoftUni.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni.Solutions
{
    public class FindEmployeesByFristNameStartingWithSa
    {
        public static string Solution(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => x.FirstName.StartsWith("Sa",true,CultureInfo.InvariantCulture)).OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                }).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                string name = employee.FirstName + ' ' + employee.LastName;
                string jobTitle = employee.JobTitle;
                decimal salary = employee.Salary;

                sb.AppendLine($"{name} - {jobTitle} - (${salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
