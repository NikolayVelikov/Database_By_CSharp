using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            string result = string.Empty;
            //DataBase(db);

            //string jsonSupplier = File.ReadAllText("../../../Datasets/suppliers.json");
            //result = ImportSuppliers(db, jsonSupplier);
            //string jsonParts = File.ReadAllText("../../../Datasets/parts.json");
            //result = ImportParts(db, jsonParts);
            //string jsonCars = File.ReadAllText("../../../Datasets/cars.json");
            //result = ImportCars(db, jsonCars);
            //string jsonCustomers = File.ReadAllText("../../../Datasets/customers.json");
            //result = ImportCustomers(db, jsonCustomers);
            //string jsonSales = File.ReadAllText("../../../Datasets/sales.json");
            //result = ImportSales(db, jsonSales);
            //result = GetOrderedCustomers(db);
            //result = GetCarsFromMakeToyota(db);
            //result = GetLocalSuppliers(db);
            //result = GetCarsWithTheirListOfParts(db);
            result = GetTotalSalesByCustomer(db);

            Console.WriteLine(result);
        }
        public static void DataBase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine("Created!!!");
        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var objJson = JsonConvert.DeserializeObject<Supplier[]>(inputJson);
            context.AddRange(objJson);

            context.SaveChanges();

            return $"Successfully imported {objJson.Length}.";
        }
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var suppliers = context.Suppliers.Select(x => x.Id).ToArray();
            var objParts = JsonConvert.DeserializeObject<Part[]>(inputJson).Where(p => suppliers.Contains(p.SupplierId)).ToArray();

            context.AddRange(objParts);

            context.SaveChanges();

            return $"Successfully imported {objParts.Length}.";
        }
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var objCars = JsonConvert.DeserializeObject<CarInputModel[]>(inputJson);

            var carlist = new List<Car>();
            foreach (var car in objCars)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };

                foreach (var partId in car?.PartsId.Distinct())
                {
                    currentCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                carlist.Add(currentCar);
            }

            context.AddRange(carlist);

            context.SaveChanges();

            return $"Successfully imported {carlist.Count}.";
        }
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var objCustomers = JsonConvert.DeserializeObject<Customer[]>(inputJson);
            context.AddRange(objCustomers);

            context.SaveChanges();

            return $"Successfully imported {objCustomers.Length}.";
        }
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var objSales = JsonConvert.DeserializeObject<Sale[]>(inputJson);
            context.AddRange(objSales);

            context.SaveChanges();

            return $"Successfully imported {objSales.Length}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                }).ToArray();

            var result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;
        }
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .Where(t => t.Make == "Toyota")
                .OrderBy(m => m.Model)
                .ThenByDescending(d => d.TravelledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                }).ToArray();

            var result = JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);

            return result;
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                }).ToArray();

            var result = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return result;
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance,
                    },
                    parts = c.PartCars.Select(p => new
                    {
                        Name = p.Part.Name,
                        Price = $"{p.Part.Price:f2}"
                    }).ToArray()
                }).ToArray();

            var result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers.Where(x => x.Sales.Select(car => car.Car).Count() >= 1).Select(c => new
            {
                fullName = c.Name,
                boughtCars = c.Sales.Select(car => car.Car).Count(),
                spentMoney = c.Sales
                        .Select(s => s.Car.PartCars
                                            .Select(m => m.Part.Price)
                                            .Sum())
                        .Sum()
            }).OrderByDescending(x=> x.spentMoney).ThenByDescending(x=> x.boughtCars).ToArray();            

            var objCustomers = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return objCustomers;
        }
    }
}