namespace BookShop
{
    using System;

    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //Console.WriteLine(GetBooksByAgeRestriction(db, Console.ReadLine()));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksByPrice(db));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            return AgeRestrictions.Solution(context, command);
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            return GoldenBooks.Solution(context);
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            return BooksByPrice.Solution(context);
        }
    }
}
