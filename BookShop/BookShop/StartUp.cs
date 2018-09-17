namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // DbInitializer.ResetDatabase(db)

                //string command = Console.ReadLine(); ;
                //string result = GetBooksByAgeRestriction(db, command);

                //string result = GetGoldenBooks(db);

                //string result = GetBooksByPrice(db);

                //int year = int.Parse(Console.ReadLine());
                //string result = GetBooksNotRealeasedIn(db, year);

                //string input = Console.ReadLine();
                //string result = GetBooksByCategory(db, input);

                //string date = Console.ReadLine();
                //string result = GetBooksReleasedBefore(db, date);
                // Console.WriteLine(result.Length);

                //string input = Console.ReadLine();
                //string result = GetAuthorNamesEndingIn(db, input);

                //string input = Console.ReadLine();
                //string result = GetBookTitlesContaining(db, input);

                //string input = Console.ReadLine();
                //string result = GetBooksByAuthor(db, input);

                //int lengthCheck = int.Parse(Console.ReadLine());
                //int result = CountBooks(db, lengthCheck);

                //string result = CountCopiesByAuthor(db);

                // string result = GetTotalProfitByCategory(db);

                //string result = GetMostRecentBooks(db);

                //IncreasePrices(db);

                int result = RemoveBooks(db);
                Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction restriction = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);
           
            var books = context.Books
                .Where(x => x.AgeRestriction == restriction)
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToArray();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var result = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x =>  $"{x.Title} - ${x.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ").Select(x => x.ToLower()).ToArray();

            var books = context.Books
                .Where(x => x.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToArray();


            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime time = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(x => x.ReleaseDate < time)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => $"{x.Title} - {x.EditionType} - ${x.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => $"{x.FirstName} {x.LastName}")
                .OrderBy(x => x)
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksAuthors = context.Books
                //.Where(x => EF.Functions.Like(x.Author.FirstName, "%" + input))
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower())) 
                .OrderBy(x => x.BookId)
                .Select(x => $"{x.Title} ({x.Author.FirstName} {x.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, booksAuthors);
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var countBooks = context.Books
                .Where(x => x.Title.Count() > lengthCheck)
                .Count();

            return countBooks;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var list = context.Authors
                .OrderByDescending(x => x.Books.Sum(c => c.Copies))
                .Select(x => $"{x.FirstName} {x.LastName} - {x.Books.Sum(c => c.Copies)}")
                .ToArray();

            return string.Join(Environment.NewLine, list);
               
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    Name = x.Name,
                    TotalProfit = x.CategoryBooks.Sum(s => s.Book.Price * s.Book.Copies)
                })
                .OrderByDescending(x => x.TotalProfit)
                .ThenBy(x => x.Name)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var c in categories)
            {
                sb.AppendLine($"{c.Name} ${c.TotalProfit:f2}");
            }
            return sb.ToString().Trim();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    CategoryName = x.Name,
                    Books = x.CategoryBooks                                 
                                 .Select(s => new
                                 {
                                     Title = s.Book.Title,
                                     ReleaseDate = s.Book.ReleaseDate
                                 })
                                 .OrderByDescending(r => r.ReleaseDate)
                                 .Take(3)
                                 .ToArray()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var item in categories)
            {
                sb.AppendLine($"--{item.CategoryName}");
                foreach (var kvp in item.Books)
                {
                    sb.AppendLine($"{kvp.Title} ({kvp.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var item in books)
            {
                item.Price += 5;
            }
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var removedBooks = context.Books.Where(x => x.Copies < 4200).ToArray();

            context.Books.RemoveRange(removedBooks);
            context.SaveChanges();

            return removedBooks.Length;
        }
    }
}
