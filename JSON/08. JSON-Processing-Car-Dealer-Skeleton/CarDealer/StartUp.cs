using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
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
            string jsonSupplier = File.ReadAllText("../../../Datasets/suppliers.json");
            result = ImportSuppliers(db, jsonSupplier);

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

            return null;
        }
    }
}