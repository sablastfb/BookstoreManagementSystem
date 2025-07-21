using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class Deatails
{
  public record BookDetailEnvelope(
    string Title,
    decimal Price,
    double AverageRating,
    List<string> AuthorNames,
    List<string> GenreNames);

  public record Query(Guid Id) : IRequest<BookDetailEnvelope>;

  public class Handler(BookstoreDbContext context) : IRequestHandler<Query, BookDetailEnvelope>
  {
    public async Task<BookDetailEnvelope> Handle(Query request, CancellationToken cancellationToken)
    {
      var book = await context.Books
        .Where(b => b.Id == request.Id)
        .Include(b => b.BookAuthors)
        .ThenInclude(ba => ba.Author)
        .Include(b => b.BookGenres)
        .ThenInclude(bg => bg.Genre)
        .Include(b => b.Reviews)
        .Include(b => b.Reviews)
        .Select(b => new BookDetailEnvelope
        (
          b.Title,
          b.Price,
          b.Reviews.Any() ? b.Reviews.Average(r => (double)r.Rating) : 0,
          b.BookAuthors.Select(ba => ba.Author!.Name).ToList(),
          b.BookGenres.Select(bg => bg.Genre!.Name).ToList()
        ))
        .AsNoTracking()
        .FirstOrDefaultAsync(cancellationToken);

      if (book == null)
      {
        throw new RestException(
          HttpStatusCode.NotFound,
          new { Book = Constants.NOT_FOUND }
        );
      }

      return book;
    }
  }
}
