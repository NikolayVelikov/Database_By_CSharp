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
            Console.WriteLine(GetAddressesByTown(softUniContext));
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
