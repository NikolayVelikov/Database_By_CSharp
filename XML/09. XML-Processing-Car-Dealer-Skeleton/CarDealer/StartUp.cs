using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using CarDealer.Concerter;
using CarDealer.Data;
using CarDealer.DataTransferObjects.Input;
using CarDealer.DataTransferObjects.Output;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            string result = "work";
            ResetDatabase(db);
            
            var supplierXML = File.ReadAllText("./Datasets/suppliers.xml");
            result = ImportSuppliers(db, supplierXML);
            var partsXML = File.ReadAllText("./Datasets/parts.xml");
            result = ImportParts(db, partsXML);
            var carsXML = File.ReadAllText("./Datasets/cars.xml");
            result = ImportCars(db, carsXML);
            var customersXml = File.ReadAllText("./Datasets/customers.xml");
            result = ImportCustomers(db, customersXml);
            var salesXml = File.ReadAllText("./Datasets/sales.xml");
            result = ImportSales(db, salesXml);

            result = GetCarsWithDistance(db);
            result = GetCarsFromMakeBmw(db);
            result = GetLocalSuppliers(db);
            result = GetCarsWithTheirListOfParts(db);
            result = GetTotalSalesByCustomer(db);
            result = GetSalesWithAppliedDiscount(db);


            Console.WriteLine(result);
        }
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            int distace = 2000000;
            var cars = context.Cars
                .Where(d => d.TravelledDistance > distace)
                .Select(x => new CarOutputModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Make).ThenBy(x => x.Model).Take(10).ToArray();

            string root = "cars";
            var carsXml = XmlConverter.Serialize<CarOutputModel>(cars, root);

            return carsXml;
        }
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            string make = "BMW";

            var bmwCars = context.Cars
                .Where(x => x.Make.ToUpper() == make)
                .Select(x => new BMWOutputModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model).ThenByDescending(x => x.TravelledDistance).ToArray();

            string root = "cars";
            var bmwCarsXml = XmlConverter.Serialize<BMWOutputModel>(bmwCars, root);

            return bmwCarsXml;
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSupplier = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new LocalSupplierOutputModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToArray();

            string root = "suppliers";
            var localSuppliersXml = XmlConverter.Serialize<LocalSupplierOutputModel>(localSupplier, root);

            return localSuppliersXml;
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(x => new
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistanced = x.TravelledDistance,
                    Parts = x.PartCars.Select(y => new
                    {
                        PartName = y.Part.Name,
                        PartPrice = y.Part.Price
                    }).OrderByDescending(y => y.PartPrice).ToArray()

                }).OrderByDescending(x => x.TravelledDistanced).ThenBy(x => x.Model).Take(5).ToArray();

            List<CarPartsOutputModel> carsConvert = new List<CarPartsOutputModel>();
            foreach (var car in cars)
            {
                CarPartsOutputModel currentCar = new CarPartsOutputModel
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistanced = car.TravelledDistanced
                };

                foreach (var part in car.Parts)
                {
                    var currentPart = new PartsListOutputModel
                    {
                        Name = part.PartName,
                        Price = part.PartPrice
                    };

                    currentCar.PartsListOutputModel.Add(currentPart);
                }

                carsConvert.Add(currentCar);
            }

            string root = "cars";
            var carsXml = XmlConverter.Serialize<List<CarPartsOutputModel>>(carsConvert, root);

            return carsXml;
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers.Where(x => x.Sales.Any())
                .Select(x => new CustomerTotalSpendMoneyOutputModel
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Select(y => new { y.Car }).Count(),
                    SpendMoneyParts = x.Sales.Select(x => x.Car).SelectMany(x => x.PartCars).Sum(x => x.Part.Price)
                })
                .OrderByDescending(x => x.SpendMoneyParts)
                .ToArray();

            string root = "customers";

            var customersXml = XmlConverter.Serialize<CustomerTotalSpendMoneyOutputModel>(customers, root);

            return customersXml;
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales.Select(x => new SaleOutputModel
            {
                Car = new CarSaleOutputModel
                {
                    Make = x.Car.Make,
                    Model = x.Car.Model,
                    TravelledDistance = x.Car.TravelledDistance
                },
                Discount = x.Discount,
                CustomerName = x.Customer.Name,
                Price = x.Car.PartCars.Sum(x => x.Part.Price),
                PriceWithDiscount = x.Car.PartCars.Sum(x => x.Part.Price) - x.Car.PartCars.Sum(x => x.Part.Price) * x.Discount / 100
            }
            ).ToList();

            var salesXml = XmlConverter.Serialize(sales, "sales");


            return salesXml;
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
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            string root = "Customers";
            var xmlObjects = XmlConverter.Deserializer<CustomersInputModel>(inputXml, root);

            var customers = xmlObjects.Select(x => new Customer
            {
                Name = x.Name,
                BirthDate = x.BirthDate,
                IsYoungDriver = x.IsYoungDriver
            }).ToArray();

            context.AddRange(customers);
            context.SaveChanges();


            return $"Successfully imported {customers.Length}";
        }
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var root = "Sales";
            var existedCars = context.Cars.Select(x => x.Id).ToArray();
            var customersObjects = XmlConverter.Deserializer<SalesInputModel>(inputXml, root);

            var sales = customersObjects.Where(x => existedCars.Contains(x.CarId)).Select(x => new Sale
            {
                Discount = x.Discount,
                CarId = x.CarId,
                CustomerId = x.CustomerId
            }).ToArray();

            context.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }
    }
}