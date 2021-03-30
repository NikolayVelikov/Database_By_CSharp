namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            string root = "Books";
            var booksXml = XmlConverter.Deserializer<BookImportModel>(xmlString, root);

            List<Book> books = new List<Book>();
            StringBuilder sb = new StringBuilder();
            foreach (var book in booksXml)
            {
                DateTime pulished;
                bool isDateValid = DateTime.TryParseExact(book.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out pulished);

                if (!IsValid(book) || !isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }                
                
                var currentBook = new Book()
                {
                    Name = book.Name,
                    Genre = Enum.Parse<Genre>(book.Genre.ToString()),
                    Pages = book.Pages,
                    Price = book.Price,
                    PublishedOn = DateTime.ParseExact(book.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                };

                books.Add(currentBook);
                sb.AppendLine(String.Format(SuccessfullyImportedBook, currentBook.Name, currentBook.Price));
            }

            context.Books.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsJson = JsonConvert.DeserializeObject<AuthorInputModel[]>(jsonString);

            List<Author> authors = new List<Author>();
            StringBuilder sb = new StringBuilder();

            foreach (var currentAuthor in authorsJson)
            {
                if (!IsValid(currentAuthor) || authors.Any(x => x.Email == currentAuthor.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var author = new Author()
                {
                    FirstName = currentAuthor.FirstName,
                    LastName = currentAuthor.LastName,
                    Phone = currentAuthor.Phone,
                    Email = currentAuthor.Email,                    
                };

                foreach (var book in currentAuthor.Books)
                {                    
                    var bookContext = context.Books.FirstOrDefault(x => x.Id == book.Id);
                    if (bookContext == null)
                    {
                        continue;
                    }

                    author.AuthorsBooks.Add(new AuthorBook { Author = author, Book = bookContext });
                }

                if (author.AuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authors.Add(author);
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, author.FirstName + ' ' + author.LastName, author.AuthorsBooks.Count));
            }

            context.Authors.AddRange(authors);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}