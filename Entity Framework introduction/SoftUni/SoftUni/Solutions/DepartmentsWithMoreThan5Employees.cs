using SoftUni.Data;
using System.Linq;
using System.Text;

namespace SoftUni.Solutions
{
    public class DepartmentsWithMoreThan5Employees
    {
        public static string Solution(SoftUniContext context)
        {
            var departments = context.Departments.Where(x => x.Employees.Count > 5).OrderBy(x => x.Employees.Count).ThenBy(x => x.Name).Select(x => new
            {
                DepartmentName = x.Name,
                Manager = x.Manager,
                Employee = x.Employees.Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle
                }).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList()
            }).ToArray();

            StringBuilder sb = new StringBuilder();
            string modelOutput = "{0} - {1}";
            string nameModel = "{0} {1}";

            foreach (var department in departments)
            {
                string departmentName = department.DepartmentName;
                string managerName = string.Format(nameModel, department.Manager.FirstName, department.Manager.LastName);

                sb.AppendLine(string.Format(modelOutput, departmentName, managerName));

                //var employees = department.Employee.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToArray();
                var employees = department.Employee;
                foreach (var employee in employees)
                {
                    string employeeName = string.Format(nameModel, employee.FirstName, employee.LastName);
                    string employeeJobTitle = employee.JobTitle;

                    sb.AppendLine(string.Format(modelOutput, employeeName, employeeJobTitle));
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
