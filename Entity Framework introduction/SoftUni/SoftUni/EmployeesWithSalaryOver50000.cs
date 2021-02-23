using SoftUni.Data;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class EmployeesWithSalaryOver50000
    {
        public static string Solution(SoftUniContext context)
        {
            var emoloyeesWithSalaryOver50000 = context.Employees.Select(x => new
            {
                x.FirstName,
                x.Salary
            }
            ).Where(x => x.Salary > 50000).OrderBy(x => x.FirstName);

            StringBuilder sb = new StringBuilder();
            foreach (var employee in emoloyeesWithSalaryOver50000)
            {
                string firstName = employee.FirstName;
                decimal salary = employee.Salary;

                sb.AppendLine($"{firstName} - {salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
