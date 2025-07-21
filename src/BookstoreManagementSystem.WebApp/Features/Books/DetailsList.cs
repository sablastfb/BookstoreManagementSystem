using BookstoreManagementSystem.WebApp.Features.Books.Data;
using BookstoreManagementSystem.WebApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class DetailsList
{
  public record Query(
    int? Limit,
    int? Offset) : IRequest<BooksDetailsEnvelope>;

  public class Handler(BookstoreDbContext context) : IRequestHandler<Query, BooksDetailsEnvelope>
  {
    public async Task<BooksDetailsEnvelope> Handle(Query request, CancellationToken cancellationToken)
    {
      var booksDetails = await context.Books
        .Include(b => b.BookAuthors)
        .ThenInclude(ba => ba.Author)
        .Include(b => b.BookGenres)
        .ThenInclude(bg => bg.Genre)
        .Include(b => b.Reviews)
        .AsNoTracking()
        // Order by average rating first (this can be translated to SQL)
        .OrderByDescending(b => b.Reviews.Any() ? b.Reviews.Average(r => (double)r.Rating) : 0)
        .Skip(request.Offset ?? 0)
        .Take(request.Limit ?? 10)
        .ToListAsync(cancellationToken);

      // Then project to your envelope (in memory)
      var result = booksDetails.Select(b => new BookDetailEnvelope(
        b.Title,
        b.Price,
        b.Reviews.Any() ? b.Reviews.Average(r => (double)r.Rating) : 0,
        b.BookAuthors.Select(ba => ba.Author!.Name).ToList(),
        b.BookGenres.Select(bg => bg.Genre!.Name).ToList()
      )).ToList();

      return new BooksDetailsEnvelope( result );
    }
  }
}
