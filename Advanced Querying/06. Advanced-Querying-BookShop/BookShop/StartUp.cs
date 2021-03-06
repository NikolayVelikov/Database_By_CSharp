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

            System.Console.WriteLine(GetBooksByAgeRestriction(db, System.Console.ReadLine()));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            return AgeRestrictions.Solution(context, command);
        }
    }
}
