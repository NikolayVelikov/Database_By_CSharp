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
            //Nulling(db);
            //
            //string jsonUsers = File.ReadAllText(JSONLinks.users); // Task 1
            //result = ImportUsers(db, jsonUsers); // Task 1
            //string jsonProducts = File.ReadAllText(JSONLinks.products); // Task 2
            //result = ImportProducts(db, jsonProducts); // Task 2
            //string jsonCategories = File.ReadAllText(JSONLinks.categories); // Task 3
            //result = ImportCategories(db, jsonCategories); // Task 3
            //string jsonCategoriesToProducts = File.ReadAllText(JSONLinks.categoriesToProducts); // Task 4
            //result = ImportCategoryProducts(db, jsonCategoriesToProducts); // Taks 4

            //result = GetProductsInRange(db); // Taks 5
            //result = GetSoldProducts(db); // Task 6
            //result = GetCategoriesByProductsCount(db); // Task 7
            result = GetUsersWithProducts(db); // Task 8


            Console.WriteLine(result);
        }
        public static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Deleted!!!");
            db.Database.EnsureCreated();
            Console.WriteLine("Created!!!");
        }
        public static void Nulling(ProductShopContext db)
        {
            string jsonUsers = File.ReadAllText(JSONLinks.users); // Task 1
            ImportUsers(db, jsonUsers); // Task 1
            string jsonProducts = File.ReadAllText(JSONLinks.products); // Task 2
            ImportProducts(db, jsonProducts); // Task 2
            string jsonCategories = File.ReadAllText(JSONLinks.categories); // Task 3
            ImportCategories(db, jsonCategories); // Task 3
            string jsonCategoriesToProducts = File.ReadAllText(JSONLinks.categoriesToProducts); // Task 4
            ImportCategoryProducts(db, jsonCategoriesToProducts); // Taks 4
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

            var objcategories = JsonConvert.DeserializeObject<Category[]>(inputJson).Where(c => c.Name != null).ToArray();
            context.AddRange(objcategories);

            context.SaveChanges();

            return $"Successfully imported {objcategories.Length}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var objCategoriesToProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            context.AddRange(objCategoriesToProducts);

            context.SaveChanges();

            return $"Successfully imported {objCategoriesToProducts.Length}";
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .OrderBy(p => p.price).ToArray();

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var sold = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(y => new
                    {
                        name = y.Name,
                        price = y.Price,
                        buyerFirstName = y.Buyer.FirstName,
                        buyerLastName = y.Buyer.LastName

                    }).ToArray()
                }).ToArray();

            var outputJson = JsonConvert.SerializeObject(sold, Formatting.Indented);

            return outputJson;
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoryProducts.Select(y => y.Product).Count(),
                    averagePrice = $"{x.CategoryProducts.Select(y => y.Product.Price).Average():f2}",
                    totalRevenue = $"{x.CategoryProducts.Select(y => y.Product.Price).Sum():f2}"
                })
                .OrderByDescending(x => x.productsCount).ToArray();

            var output = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return output;
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(y => y.Buyer != null))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = new
                    {
                        count = x.ProductsSold.Count,
                        products = x.ProductsSold.Where(b => b.Buyer != null).Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        }).ToArray()
                    }
                }).OrderByDescending(x => x.soldProducts).ToArray();

            var result = new
            {
                usersCount = users.Length,
                users = users
            };

            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var json = JsonConvert.SerializeObject(result, setting);

            return json;
        }
    }
}