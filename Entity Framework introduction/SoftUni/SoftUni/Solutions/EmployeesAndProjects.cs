using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using System.Linq;
using System.Text;

namespace SoftUni.Solutions
{
    public static class EmployeesAndProjects
    {
        public static string Solution(SoftUniContext context)
        {
            var employees = context.Employees.Include(x => x.EmployeesProjects).ThenInclude(x => x.Project)
                .Where(x => x.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003)).
                Select(x => new
                {
                    EmployeeFirstName = x.FirstName,
                    EmployeeLastName = x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(y => new
                    {
                        ProjectName = y.Project.Name,
                        ProjectStart = y.Project.StartDate,
                        ProjectEnd = y.Project.EndDate
                    }
                    )
                }
                ).Take(10).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                string firsName = employee.EmployeeFirstName;
                string lastName = employee.EmployeeLastName;

                string managerFirstName = employee.ManagerFirstName;
                string managerLastName = employee.ManagerLastName;

                sb.AppendLine($"{firsName} {lastName} - Manager: {managerFirstName} {managerLastName}");
                foreach (var project in employee.Projects)
                {
                    string projectName = project.ProjectName;
                    var startDate = project.ProjectStart;
                    var endDate = project.ProjectEnd.HasValue ? project.ProjectEnd.ToString()  :"not finished";
                    if (project.ProjectEnd.HasValue)
                    {
                        sb.AppendLine($"--{projectName} - {startDate} - {endDate}");
                    }
                    else
                    {
                        sb.AppendLine($"--{projectName} - {startDate} - not finished");
                    }                    
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
