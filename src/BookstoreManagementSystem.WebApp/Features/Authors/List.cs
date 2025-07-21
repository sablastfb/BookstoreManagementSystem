using BookstoreManagementSystem.WebApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public class List
{
  public record Query(
    int? Limit,
    int? Offset) : IRequest<AuthorsEnvelope>;

  public class Handler(BookstoreDbContext context) : IRequestHandler<Query, AuthorsEnvelope>
  {
    public async Task<AuthorsEnvelope> Handle(Query message, CancellationToken cancellationToken)
    {
      var author = await context.Authors
        .OrderBy(a => a.CreatedAt)
        .Skip(message.Offset ?? 0)
        .Take(message.Limit ?? 20)
        .AsNoTracking()
        .ToListAsync(cancellationToken);

      var authorsEnvelope = new AuthorsEnvelope(author);
      authorsEnvelope.Count = await context.Authors
        .AsNoTracking()
        .CountAsync(cancellationToken);
      return authorsEnvelope;
    }
  }
}
