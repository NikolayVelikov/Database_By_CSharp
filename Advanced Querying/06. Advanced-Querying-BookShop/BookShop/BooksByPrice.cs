namespace BookShop
{
    using System.Text;
    using System.Linq;

    using BookShop.Data;

    public class BooksByPrice
    {
        public static string Solution(BookShopContext context)
        {
            decimal price = ConstantInputs.lookedForPrice;
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
    }
}
