using SoftUni.Data;
using System.Linq;
using System.Text;

namespace SoftUni.Solutions
{
    public class IncreasesSalaries
    {
        public static string Solution(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => x.Department.Name == "Engineering" || x.Department.Name == "Tool Design" || x.Department.Name == "Marketing" || x.Department.Name == "Information Services").Select(x => new
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Salary = x.Salary * 1.12m,
                Department = x.Department
            }).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                string name = employee.FirstName + " " + employee.LastName;
                decimal salary = employee.Salary;

                sb.AppendLine($"{name} (${salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
