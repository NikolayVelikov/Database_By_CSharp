using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Links;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            string result = string.Empty;
            //ResetDatabase(db);
            //
            //string jsonUsers = File.ReadAllText(JSONLinks.users); // Task 1
            //result = ImportUsers(db, jsonUsers); // Task 1
            //string jsonProducts = File.ReadAllText(JSONLinks.products); // Task 2
            //result = ImportProducts(db, jsonProducts); // Task 2
            string jsonCategories = File.ReadAllText(JSONLinks.categories); // Task 3
            result = ImportCategories(db, jsonCategories); // Task 3

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
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var objProducts = JsonConvert.DeserializeObject<Product[]>(inputJson);
            context.AddRange(objProducts);

            context.SaveChanges();

            return $"Successfully imported {objProducts.Length}";
        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {

            var objcategories = JsonConvert.DeserializeObject<Category[]>(inputJson).Where(c=> c.Name != null).ToArray();
            context.AddRange(objcategories);

            context.SaveChanges();

            return $"Successfully imported {objcategories.Length}";
        }
    }
}