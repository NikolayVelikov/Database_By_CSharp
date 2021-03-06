namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //System.Console.WriteLine(GetBooksByAgeRestriction(db, System.Console.ReadLine()));
            System.Console.WriteLine(GetGoldenBooks(db));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            return AgeRestrictions.Solution(context, command);
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            return GoldenBooks.Solution(context);
        }
    }
}
