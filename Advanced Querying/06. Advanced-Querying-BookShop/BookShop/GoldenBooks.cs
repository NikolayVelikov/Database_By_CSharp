namespace BookShop
{
    using System.Text;
    using System.Linq;

    using BookShop.Data;

    public class GoldenBooks
    {
        private const int copies = ConstantInputs.copies;

        public static string Solution(BookShopContext context)
        {
            var goldenBooks = context.Books.
                                    Where(x => x.Copies < copies).
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
    }
}
