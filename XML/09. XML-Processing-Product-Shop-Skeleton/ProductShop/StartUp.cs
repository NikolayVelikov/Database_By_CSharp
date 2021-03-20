using ProductShop.Data;
using ProductShop.Models;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using ProductShop.XMLConvert;
using ProductShop.ModelsDto.Input;
using System.Linq;
using ProductShop.ModelsDto.Output;
using System.Collections.Generic;

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
            //var xmlCategoriesInput = File.ReadAllText(@"./Datasets/categories.xml");
            //result = ImportCategories(db, xmlCategoriesInput); 
            //var xmlCategoriesInput = File.ReadAllText(@"./Datasets/categories-products.xml");
            //result = ImportCategoryProducts(db, xmlCategoriesInput);

            //result = GetProductsInRange(db);
            //result = GetSoldProducts(db);
            result = GetUsersWithProducts(db);

            Console.WriteLine(result);
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            int min = 500;
            int max = 1000;

            var products = context.Products
                                    .Where(x => x.Price >= min && x.Price <= max)
                                    .Select(x => new ProductInPriceRangeOutputModel
                                    {
                                        Name = x.Name,
                                        Price = x.Price,
                                        Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName
                                    })
                                    .OrderBy(x => x.Price).Take(10).ToArray();

            string root = "Products";
            var productsXml = XmlConverter.Serialize(products, root);

            return productsXml;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                                .Where(x => x.ProductsSold.Any())
                                .OrderBy(x => x.LastName)
                                .ThenBy(x => x.FirstName)
                                .Take(5)
                                .Select(x => new
                                {
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    Products = x.ProductsSold.Select(p => new
                                    {
                                        p.Name,
                                        p.Price
                                    }).ToArray()
                                })
                                .ToArray();

            List<UsersWithSoldProductsOutputModel> usersXml = new List<UsersWithSoldProductsOutputModel>();
            foreach (var user in users)
            {
                var currentUser = new UsersWithSoldProductsOutputModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };

                foreach (var product in user.Products)
                {
                    currentUser.Products.Add(new SoldProduct
                    {
                        Name = product.Name,
                        Price = product.Price
                    });
                }

                usersXml.Add(currentUser);
            }

            string root = "Users";
            var output = XmlConverter.Serialize(usersXml, root);

            return output;
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(p => new CategoriesOutputModel
                {
                    Name = p.Name,
                    NumberOfProducts = p.CategoryProducts.Count,
                    AveragePrice = p.CategoryProducts.Select(x => x.Product.Price).Average(),
                    TotalRevenue = p.CategoryProducts.Select(x => x.Product.Price).Sum(),
                })
                .OrderByDescending(x => x.NumberOfProducts).ThenBy(x => x.TotalRevenue).ToArray();

            string root = "Categories";
            var categoriesXml = XmlConverter.Serialize(categories, root);

            return categoriesXml;
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                                .Where(x => x.ProductsSold.Any())
                                .OrderByDescending(x => x.ProductsSold.Count)
                                .Select(x => new
                                {
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    Age = x.Age,
                                    CountSoldProduct = x.ProductsSold.Count,
                                    SoldedProducts = x.ProductsSold
                                                            .Select(p => new
                                                            {
                                                                ProductName = p.Name,
                                                                Price = p.Price
                                                            }).OrderByDescending(p => p.Price).ToArray()
                                })
                                .ToArray();

            var products = users.Count();
            var result = new OutputModel
            {
                Count = products
            };
            return null;
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
            string root = "Categories";
            var categoriesXml = XmlConverter.Deserializer<CategoriesInputModel>(inputXml, root);

            var categories = categoriesXml
                            .Where(x => x.Name != "null")
                            .Select(x => new Category
                            {
                                Name = x.Name
                            }
                            ).ToArray();

            context.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var categories = context.Categories.Select(x => x.Id).ToArray();
            var products = context.Products.Select(x => x.Id).ToArray();

            string root = "CategoryProducts";
            var productsCategoriesXml = XmlConverter.Deserializer<CategoryProductInputModel>(inputXml, root);

            var productsCategories = productsCategoriesXml
                .Where(x => categories.Contains(x.CategoryId) && products.Contains(x.ProductId))
                .Select(x => new CategoryProduct
                {
                    CategoryId = x.CategoryId,
                    ProductId = x.ProductId
                }).ToArray();

            context.AddRange(productsCategories);
            context.SaveChanges();

            return $"Successfully imported {productsCategories.Length}";
        }
    }
}