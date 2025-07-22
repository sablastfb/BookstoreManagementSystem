using Bogus;
using BookstoreDataGenerator.Models;

namespace BookstoreDataGenerator.Utils;

public static class BookstoreDataFaker
{
  public static List<Author> GenerateFakeAuthors(int count)
  {
    var authorFaker = new Faker<Author>()
      .RuleFor(a => a.Name, f => f.Name.FullName())
      .RuleFor(a => a.BirthYear, f => f.Date.Past(80).Year);

    return authorFaker.Generate(count);
  }

  private static readonly string[] CommonGenres = 
  {
    "Fantasy", "Science Fiction", "Mystery", "Thriller", "Romance",
    "Horror", "Historical", "Biography", "Dystopian", "Young Adult",
    "Adventure", "Crime", "Comedy", "Drama", "Non-Fiction"
  };
  public static List<Genre> GenerateFakeGenres()
  {
    return CommonGenres.Select(g => new Genre { Name = g }).ToList();
  }

  public static List<Book> GenerateFakeBook(int count, List<AuthorsResponse.AuthorData> Authors, List<GenreResponse.GenreData> Genres )
  {
    var bookFaker = new Faker<Book>()
      .RuleFor(b => b.BookData, f => new BookData
      {
        Price = Math.Round(f.Random.Decimal(5, 100), 2),
        Title = f.Commerce.ProductName() + " " + f.Random.Word(),
        AuthorList = f.Random.ListItems(Authors,f.Random.Int(1, 3))
          .Select(a => a.Id)
          .ToList(),
        GenreList = f.Random.ListItems(Genres, f.Random.Int(1, 3))
          .Select(g => g.Id) 
          .ToList()
      });

    return bookFaker.Generate(count);
  }

  public static List<Review> GenerateFakeReviews(int count, List<BookDto> books )
  {
    var reviewFaker = new Faker<Review>()
      .RuleFor(a => a.Rating, f => f.Random.Short(1, 5))
      .RuleFor(r => r.BookId, f =>
      {
        var randomBook = f.PickRandom(books);
        return randomBook.Id;
      });
    
    return reviewFaker.Generate(count);
  }
}
