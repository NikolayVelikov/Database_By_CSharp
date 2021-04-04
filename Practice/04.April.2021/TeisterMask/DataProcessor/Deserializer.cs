namespace TeisterMask.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            string root = "Projects";
            var projectsXml = XmlConverter.Deserializer<ProjectImportModel>(xmlString, root);

            StringBuilder sb = new StringBuilder();
            List<Project> projects = new List<Project>();
            foreach (var project in projectsXml)
            {
                if (!IsValid(project))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }                
                
                bool dueDateIf = DateTime.TryParseExact(project.DueDate, "dd/MM/yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime dueDate);                

                var currentProject = new Project()
                {
                    Name = project.Name,
                    OpenDate = DateTime.ParseExact(project.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DueDate = dueDateIf ? dueDate : (DateTime?)null,
                };

                foreach (var task in project.Tasks)
                {
                    if (!IsValid(task))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var openDateTask = DateTime.ParseExact(task.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (openDateTask < currentProject.OpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var dueDateTask = DateTime.ParseExact(task.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (currentProject.DueDate.HasValue)
                    {
                        if (dueDateTask > dueDate)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }
                    }

                    ExecutionType executionType;
                    LabelType labelType;
                    bool executType = Enum.TryParse<ExecutionType>(task.ExecutionType, out executionType);
                    bool label = Enum.TryParse<LabelType>(task.LabelType, out labelType);
                    if (!executType || !label)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var currentTask = new Task()
                    {
                        Name = task.Name,
                        OpenDate = openDateTask,
                        DueDate = dueDateTask,
                        ExecutionType = executionType,
                        LabelType = labelType
                    };

                    currentProject.Tasks.Add(currentTask);
                }

                projects.Add(currentProject);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, currentProject.Name, currentProject.Tasks.Count));
            }
            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeesJson = JsonConvert.DeserializeObject<EmployeeInputModel[]>(jsonString);

            var existedTask = context.Tasks.Select(x => x.Id).ToArray();

            StringBuilder sb = new StringBuilder();
            List<Employee> employees = new List<Employee>();
            foreach (var employee in employeesJson)
            {
                if (!IsValid(employee))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                };

                var currentEmployee = new Employee()
                {
                    Username = employee.Username,
                    Email = employee.Email,
                    Phone = employee.Phone
                };

                var tasks = employee.Tasks.Distinct().ToArray();
                foreach (var taskId in tasks)
                {
                    if (!existedTask.Contains(taskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var employeeTask = new EmployeeTask()
                    {
                        TaskId = taskId
                    };

                    currentEmployee.EmployeesTasks.Add(employeeTask);
                }

                employees.Add(currentEmployee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee,currentEmployee.Username,currentEmployee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}