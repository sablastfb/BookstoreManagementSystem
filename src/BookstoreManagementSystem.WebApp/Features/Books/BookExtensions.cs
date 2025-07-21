using BookstoreManagementSystem.WebApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public static class BookExtensions
{
  public static IQueryable<Book> GetAllData(this DbSet<Book> books) =>
    books
      .Include(x => x.Reviews)
      .Include(x => x.BookAuthors)
      .Include(x => x.BookGenres)
      .AsNoTracking();
}
