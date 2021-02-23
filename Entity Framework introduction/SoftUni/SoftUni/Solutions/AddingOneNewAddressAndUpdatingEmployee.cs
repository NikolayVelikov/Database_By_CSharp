using SoftUni.Data;
using SoftUni.Models;
using System.Linq;
using System.Text;

namespace SoftUni.Solutions
{
    public class AddingOneNewAddressAndUpdatingEmployee
    {
        public static string Solution(SoftUniContext context)
        {
            string input = "Vitoshka 15";
            var vitosha = new Address()
            {
                AddressText = input,
                TownId = 4
            };
            context.Addresses.Add(vitosha);

            context.SaveChanges();

            var employeeNakov = context.Employees.First(x => x.LastName == "Nakov");
            employeeNakov.AddressId = vitosha.AddressId;

            context.SaveChanges();

            var employees = context.Employees.Select(x => new
            {
                x.Address.AddressId,
                x.Address.AddressText
            }).OrderByDescending(x => x.AddressId).Take(10).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine(employee.AddressText);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
