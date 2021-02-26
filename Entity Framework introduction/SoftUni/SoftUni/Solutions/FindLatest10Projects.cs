using SoftUni.Data;
using System.Linq;
using System.Text;

namespace SoftUni.Solutions
{
    public class FindLatest10Projects
    {
        public static string Solution(SoftUniContext context)
        {
            var projects = context.Projects.OrderByDescending(x => x.StartDate).Take(10).OrderBy(x => x.Name).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var project in projects)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }

            return sb.ToString().TrimEnd();
        }
    }
}
