namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var tasks = context.Projects
                        .Where(x => x.Tasks.Count > 0)
                        .Select(x => new ProjectsOutputModel()
                        {
                            TasksCount = x.Tasks.Count,
                            ProjectName = x.Name,
                            HasEndDate = x.DueDate.HasValue ? "Yes" : "No",
                            Tasks = x.Tasks
                                    .Select(t => new TaskOutputModel()
                                    {
                                        Name = t.Name,
                                        Label = t.LabelType.ToString()
                                    })
                                    .OrderBy(t=> t.Name)
                                    .ToArray()
                        })
                        .OrderByDescending(x=> x.TasksCount).ThenBy(x=> x.ProjectName).ToArray();
            
            string root = "Projects";
            var outputXml = XmlConverter.Serialize(tasks, root);

            return outputXml.TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                                    .ToArray()
                                    .Select(x => new
                                    {
                                        Username = x.Username,
                                        Tasks = x.EmployeesTasks.Where(t => t.Task.OpenDate >= date).Select(y => new
                                        {
                                            TaskName = y.Task.Name,
                                            OpenDate = y.Task.OpenDate.ToString("MM/dd/yyyy"),
                                            DueDate = y.Task.DueDate.ToString("MM/dd/yyyy"),
                                            LabelType = y.Task.LabelType.ToString(),
                                            ExecutionType = y.Task.ExecutionType.ToString()
                                        })
                                        .OrderByDescending(t => DateTime.ParseExact(t.DueDate, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                                        .ThenBy(t => t.TaskName).ToList()
                                    })
                                    .Where(x => x.Tasks.Count > 0)
                                    .OrderByDescending(x => x.Tasks.Count)
                                    .ThenBy(x => x.Username).Take(10).ToList();

            var employeesJson = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return employeesJson;
        }
    }
}