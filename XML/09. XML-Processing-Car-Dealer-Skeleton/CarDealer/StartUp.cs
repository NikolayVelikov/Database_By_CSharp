using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using CarDealer.Data;
using CarDealer.DataTransferObjects.Input;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            string result = string.Empty;
            //ResetDatabase(db);
            var supplierXML = File.ReadAllText("./Datasets/suppliers.xml");
            result = ImportSuppliers(db, supplierXML);


            Console.WriteLine(result);
            
        }
        private static void ResetDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            Console.WriteLine("Deleted!!!");
            context.Database.EnsureCreated();
            Console.WriteLine("Created!!!");
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SupplierInputModel[]),new XmlRootAttribute("Suppliers"));
            var textRead = new StringReader(inputXml);

            var suppliersDTO = xmlSerializer.Deserialize(textRead) as SupplierInputModel[];

            var suppliers = suppliersDTO.Select(x => new Supplier { Name = x.Name, IsImporter = x.IsImporter }).ToArray();

            context.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {


            return null;
        }
    }
}