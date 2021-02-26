using SoftUni.Data;
using SoftUni.Models;
using System.Linq;
using System.Text;

namespace SoftUni.Solutions
{
    public class DeletesProjectById
    {
        public static string Solution(SoftUniContext context)
        {
            Project project = context.Projects.First(x => x.ProjectId == 2);
            EmployeeProject ep = context.EmployeesProjects.First(x => x.Project == project);
            context.EmployeesProjects.Remove(ep);
            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects.Take(10).Select(x => new
            {
                x.Name
            }).ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var item in projects)
            {
                sb.AppendLine(item.Name);
            }

            return sb.ToString().TrimEnd();            
        }
    }
}
