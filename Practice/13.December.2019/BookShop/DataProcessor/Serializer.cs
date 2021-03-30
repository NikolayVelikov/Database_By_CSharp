namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors.ToList()
                .Select(a => new
                {
                    AuthorName = a.FirstName + ' ' + a.LastName,
                    Books = a.AuthorsBooks
                    .OrderByDescending(b=> b.Book.Price)
                    .Select(b => new
                    {
                        BookName = b.Book.Name,
                        BookPrice = b.Book.Price.ToString("f2")
                    }).ToArray()
                })
                .ToArray()
                .OrderByDescending(a => a.Books.Length)
                .ThenBy(a => a.AuthorName);

            var authorsJson = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return authorsJson;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            Genre genre = Enum.Parse<Genre>("science",true);
            var books = context.Books
                .Where(x => x.PublishedOn <= date && x.Genre == genre)                
                .Select(x => new BookOutputModel()
                {
                    Pages = x.Pages.ToString(),
                    Name = x.Name,
                    Date = x.PublishedOn.ToString("MM/dd/yyyy")
                })
                .OrderByDescending(x => int.Parse(x.Pages))
                .ThenByDescending(x => x.Date).Take(10).ToArray();

            var a = books.FirstOrDefault(x => x.Name == "Palo Blanco");
            string root = "Books";
            var booksXml = XmlConverter.Serialize<BookOutputModel>(books, root);

            return booksXml;
        }
    }
}