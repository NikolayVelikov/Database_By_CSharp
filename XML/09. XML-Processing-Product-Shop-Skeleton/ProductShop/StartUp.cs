using ProductShop.Data;
using ProductShop.Models;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{    
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            //ResetDatabase(db);
            var serializer = new XmlSerializer(typeof(User[]), new XmlRootAttribute("Users"));
           
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
            return null;
        }
    }
}