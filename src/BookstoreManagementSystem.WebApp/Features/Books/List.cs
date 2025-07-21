using BookstoreManagementSystem.WebApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class List
{
  public record Query(
    int? Limit,
    int? Offset) : IRequest<BooksEnvelope>;

  public class Handler(BookstoreDbContext context) : IRequestHandler<Query, BooksEnvelope>
  {
    public async Task<BooksEnvelope> Handle(Query request, CancellationToken cancellationToken)
    {
      var books = await context.Books
        .OrderBy(b => b.CreatedAt)
        .Skip(request.Offset ?? 0)
        .Take(request.Limit ?? 20)
        .AsNoTracking()
        .ToListAsync(cancellationToken);
      
      var booksEnvelope = new BooksEnvelope(books);
      booksEnvelope.Count = await context.Books
        .AsNoTracking()
        .CountAsync(cancellationToken);
      return booksEnvelope;
    }
  }
}
