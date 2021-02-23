using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var softUniContext = new SoftUniContext();
            
            string result = GetEmployeesFullInformation(softUniContext);
            Console.WriteLine(result);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            return EmployeesFullInformation.GetEmployeesFullInformation(context);
        }

        //public static string GetEmployeesFullInformation(SoftUniContext context)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var employees = context.Employees.Select(x => new
        //    {
        //        x.EmployeeId,
        //        x.FirstName,
        //        x.LastName,
        //        x.MiddleName,
        //        x.JobTitle,
        //        x.Salary
        //    }
        //    ).OrderBy(y => y.EmployeeId);

        //    foreach (var employee in employees)
        //    {
        //        string firsName = employee.FirstName;
        //        string lastName = employee.LastName;
        //        string middleName = employee.MiddleName;
        //        string jobTitle = employee.JobTitle;
        //        decimal salary = employee.Salary;

        //        sb.AppendLine($"{firsName} {lastName} {middleName} {jobTitle} {salary:F2}");
        //    }

        //    return sb.ToString().TrimEnd();
        //}
    }
}
