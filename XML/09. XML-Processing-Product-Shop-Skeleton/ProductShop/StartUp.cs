using ProductShop.Data;
using ProductShop.Models;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using ProductShop.XMLConvert;
using ProductShop.ModelsDto.Input;
using System.Linq;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            string result = string.Empty;

            //ResetDatabase(db);
            //var xmlUsersInput = File.ReadAllText(@"./Datasets/users.xml");
            //result = ImportUsers(db, xmlUsersInput);
            //var xmlProductsInput = File.ReadAllText(@"./Datasets/products.xml");
            //result = ImportProducts(db, xmlProductsInput);
            var xmlCategoriesInput = File.ReadAllText(@"./Datasets/categories.xml");
            result = ImportProducts(db, xmlCategoriesInput);

            Console.WriteLine(result);
        }

        private static void ResetDatabase(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            Console.WriteLine("Deleted!!!");
            context.Database.EnsureCreated();
            Console.WriteLine("Created!!!");
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            const string root = "Users";
            var usersXml = XmlConverter.Deserializer<UserInputModel>(inputXml, root);

            var users = usersXml
                .Select(x => new User
                {
                    Age = x.Age,
                    FirstName = x.FristName,
                    LastName = x.LastName
                }).ToArray();

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            const string root = "Products";
            var productsXml = XmlConverter.Deserializer<ProductInputModel>(inputXml, root);

            var products = productsXml
                .Select(x => new Product
                {
                    Name = x.Name,
                    Price = x.Price,
                    BuyerId = x.BuyerId,
                    SellerId = x.SellerId
                }
                ).ToArray();

            context.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {

            return null;
        }
    }
}