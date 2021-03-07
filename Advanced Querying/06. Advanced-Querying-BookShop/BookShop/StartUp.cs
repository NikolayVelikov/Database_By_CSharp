namespace BookShop
{
    using System;
    using System.Linq;
    using System.Text;

    using Initializer;

    using Data;
    using BookShop.Models.Enums;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //Console.WriteLine(GetBooksByAgeRestriction(db, Console.ReadLine()));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksByPrice(db));
            //Console.WriteLine(GetBooksNotReleasedIn(db,int.Parse(Console.ReadLine())));
            //Console.WriteLine(GetBooksByCategory(db, Console.ReadLine()));  
            Console.WriteLine(GetBooksByCategory(db, Console.ReadLine()));

        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var booksTitles = context.Books.
                Where(t => t.AgeRestriction == ageRestriction).
                Select(x => new
                {
                    Title = x.Title
                }).OrderBy(x => x.Title).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var title in booksTitles)
            {
                sb.AppendLine(title.Title);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            EditionType goldenType = Enum.Parse<EditionType>("Gold", true);

            var goldenBooks = context.Books.
               Where(x => x.EditionType == goldenType && x.Copies < 5000).
               Select(x => new
               {
                   Id = x.BookId,
                   Title = x.Title
               }).
               OrderBy(x => x.Id).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var title in goldenBooks)
            {
                sb.AppendLine(title.Title);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            decimal price = 40;

            var books = context.Books
                .Where(x => x.Price > price)
                .Select(x => new
                {
                    Title = x.Title,
                    Price = x.Price
                }
                ).OrderByDescending(x => x.Price).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(book => book.ReleaseDate.HasValue && book.ReleaseDate.Value.Year != year)
                .Select(x => new
                {
                    x.BookId,
                    x.Title,
                    x.ReleaseDate
                })
                .OrderBy(x => x.BookId)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()).ToArray();

            var books = context.Books
                .Where(x => x.BookCategories
                .Any(y => categories.Contains(y.Category.Name.ToLower()))).Select(x => new { x.Title })
                .OrderBy(x => x.Title)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var item in books)
            {
                sb.AppendLine(item.Title);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
