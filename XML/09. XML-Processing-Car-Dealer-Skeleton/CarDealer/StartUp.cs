using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using CarDealer.Concerter;
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

            //var supplierXML = File.ReadAllText("./Datasets/suppliers.xml");
            //result = ImportSuppliers(db, supplierXML);
            //var partsXML = File.ReadAllText("./Datasets/parts.xml");
            //result = ImportParts(db, partsXML);
            var carsXML = File.ReadAllText("./Datasets/cars.xml");
            result = ImportCars(db, carsXML);

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
            string root = "Suppliers";
            var suppliersModel = XmlConverter.Deserializer<SupplierInputModel>(inputXml, root);
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(SupplierInputModel[]),new XmlRootAttribute//("Suppliers"));
            //var textRead = new StringReader(inputXml);
            //
            //var suppliersDTO = xmlSerializer.Deserialize(textRead) as SupplierInputModel[];
            //
            //var suppliers = suppliersDTO.Select(x => new Supplier { Name = x.Name, IsImporter = x.IsImporter }).ToArray();
            var suppliers = suppliersModel.Select(x => new Supplier() { Name = x.Name, IsImporter = x.IsImporter }).ToArray();
            context.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            const string root = "Parts";

            var suppliersDto = XmlConverter.Deserializer<PartInputModel>(inputXml, root);

            var supplierIds = context.Suppliers.Select(x => x.Id).ToArray();

            var parts = suppliersDto
                .Where(s => supplierIds.Contains(s.SupplierId))
                .Select(x => new Part
                {
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    SupplierId = x.SupplierId
                }).ToArray();

            context.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            const string root = "Cars";
            var currentParts = context.Parts.Select(x => x.Id).ToArray();

            var cars = new List<Car>();
            var carsDtos = XmlConverter.Deserializer<CarInputModel>(inputXml, root);

            foreach (var currentCar in carsDtos)
            {
                var distinctedParts = currentCar.CarPartsInputModel.Select(x => x.PartId).Distinct();
                var parts = distinctedParts.Intersect(currentParts);

                Car car = new Car
                {
                    Make = currentCar.Make,
                    Model = currentCar.Model,
                    TravelledDistance = currentCar.TraveledDistance
                };

                foreach (var part in parts)
                {
                    var partCar = new PartCar
                    {
                        PartId = part
                    };
                    car.PartCars.Add(partCar);
                }
                cars.Add(car);
            }

            context.AddRange(cars);
            context.SaveChanges();
            
            return $"Successfully imported {cars.Count}";
        }
    }
}