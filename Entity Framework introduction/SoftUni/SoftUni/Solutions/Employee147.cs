using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Linq;
using System.Text;

namespace SoftUni.Solutions
{
    public class Employee147
    {
        public static string Solution(SoftUniContext context)
        {
            var agent147 = context.Employees.Select(x => new Employee
            {
                EmployeeId = x.EmployeeId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                JobTitle = x.JobTitle,
                EmployeesProjects = x.EmployeesProjects.
                Select(y => new EmployeeProject
                {
                    Project = y.Project
                }).OrderBy(x => x.Project.Name).ToArray()
            }).Where(x => x.EmployeeId == 147);

            StringBuilder sb = new StringBuilder();
            foreach (var agent in agent147)
            {
                string firstName = agent.FirstName;
                string lastName = agent.LastName;
                string jobTitle = agent.JobTitle;

                sb.AppendLine($"{firstName} {lastName} - {jobTitle}");

                foreach (var project in agent.EmployeesProjects)
                {
                    string projectName = project.Project.Name;
                    sb.AppendLine(projectName);
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
