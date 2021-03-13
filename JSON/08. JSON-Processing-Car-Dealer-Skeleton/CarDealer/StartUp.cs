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
            DataBase(db);

            string jsonSupplier = File.ReadAllText("../../../Datasets/suppliers.json"); // Task 1
            result = ImportSuppliers(db, jsonSupplier); // Task 1
            string jsonParts = File.ReadAllText("../../../Datasets/parts.json"); // Task 2
            result = ImportParts(db, jsonParts); // Task 2
            string jsonCars = File.ReadAllText("../../../Datasets/cars.json"); // Task 3
            result = ImportCars(db, jsonCars); // Task 3

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
    }
}