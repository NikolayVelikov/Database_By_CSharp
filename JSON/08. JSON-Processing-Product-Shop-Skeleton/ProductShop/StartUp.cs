using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            //ResetDatabase(db);
            string json = File.ReadAllText("../../../Datasets/users.json"); // Task 1
            string result = ImportUsers(db, json); // Task 1


            Console.WriteLine(result);
        }

        public static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Deleted!!!");
            db.Database.EnsureCreated();
            Console.WriteLine("Created!!!");
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var objUser = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.AddRange(objUser);

            context.SaveChanges();

            return $"Successfully imported {objUser.Length}";
        }
    }
}