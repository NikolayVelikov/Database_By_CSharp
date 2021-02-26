using System;
using SoftUni.Data;
using SoftUni.Solutions;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var softUniContext = new SoftUniContext();
            //Console.WriteLine(GetEmployeesFullInformation(softUniContext));
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(softUniContext));
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(softUniContext));
            //Console.WriteLine(AddNewAddressToEmployee(softUniContext));
            //Console.WriteLine(GetEmployeesInPeriod(softUniContext));
            //Console.WriteLine(GetAddressesByTown(softUniContext));
            //Console.WriteLine(GetEmployee147(softUniContext));
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(softUniContext));
            //Console.WriteLine(GetLatestProjects(softUniContext));
            //Console.WriteLine(IncreaseSalaries(softUniContext));
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(softUniContext));
            //Console.WriteLine(DeleteProjectById(softUniContext));
            Console.WriteLine(RemoveTown(softUniContext));
        }
        public static string RemoveTown(SoftUniContext context)
        {
            return RemovesTown.Solution(context);
        }
        public static string DeleteProjectById(SoftUniContext context)
        {
            return DeletesProjectById.Solution(context);
        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            return FindEmployeesByFristNameStartingWithSa.Solution(context);
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            return IncreasesSalaries.Solution(context);
        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            return FindLatest10Projects.Solution(context);
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            return DepartmentsWithMoreThan5Employees.Solution(context);
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            return Employee147.Solution(context);
        }
        public static string GetAddressesByTown(SoftUniContext context)
        {
            return AddressesByTown.Solution(context);
        }
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            return EmployeesAndProjects.Solution(context);
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            return AddingOneNewAddressAndUpdatingEmployee.Solution(context);
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            return EmployeesFromResearchAndDevelopment.Solution(context);
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            return EmployeesWithSalaryOver50000.Solution(context);
        }
        public static string GetEmployeesFullInformation(SoftUniContext context) // Task 3
        {
            return EmployeesFullInformation.Solution(context);
        }
    }
}
