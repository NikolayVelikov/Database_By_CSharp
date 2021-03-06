namespace BookShop
{
    using System;
    using System.Linq;
    using System.Text;
    using BookShop.Data;
    using BookShop.Models.Enums;


    public class AgeRestrictions
    {
        public static string Solution(BookShopContext context, string command)
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
    }
}
