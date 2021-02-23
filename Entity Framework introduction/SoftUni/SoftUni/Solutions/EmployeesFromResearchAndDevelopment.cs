using SoftUni.Data;
using System.Linq;
using System.Text;

namespace SoftUni.Solutions
{
    public static class EmployeesFromResearchAndDevelopment
    {
        public static string Solution(SoftUniContext context)
        {
            var empoloyees = context.Employees.Select(x => new
            {
                x.FirstName,
                x.LastName,
                x.Salary,
                x.Department
            }).Where(x => x.Department.Name == "Research and Development").OrderBy(x => x.Salary).ThenByDescending(x => x.FirstName).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in empoloyees)
            {
                string firsName = employee.FirstName;
                string lastName = employee.LastName;
                string departmentName = employee.Department.Name;
                decimal salary = employee.Salary;

                sb.AppendLine($"{firsName} {lastName} from {departmentName} - ${salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
