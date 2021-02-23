﻿using SoftUni.Data;
using System.Text;
using System.Linq;

namespace SoftUni
{
    public class EmployeesFullInformation
    {
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees.Select(x => new
            {
                x.EmployeeId,
                x.FirstName,
                x.LastName,
                x.MiddleName,
                x.JobTitle,
                x.Salary
            }
            ).OrderBy(y => y.EmployeeId);

            foreach (var employee in employees)
            {
                string firsName = employee.FirstName;
                string lastName = employee.LastName;
                string middleName = employee.MiddleName;
                string jobTitle = employee.JobTitle;
                decimal salary = employee.Salary;

                sb.AppendLine($"{firsName} {lastName} {middleName} {jobTitle} {salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
