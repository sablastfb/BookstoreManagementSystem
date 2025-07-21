using BookstoreManagementSystem.WebApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public class List
{
  public record Query() : IRequest<AuthorsEnvelope>;

  public class Handler(BookstoreDbContext context) : IRequestHandler<Query, AuthorsEnvelope>
  {
    public async Task<AuthorsEnvelope> Handle(Query request, CancellationToken cancellationToken)
    {
      var author = await context.Authors
        .AsNoTracking()
        .OrderBy(a => a.Name)
        .ToListAsync(cancellationToken);;

      var authorsEnvelope = new AuthorsEnvelope(author);
      authorsEnvelope.Count = authorsEnvelope.Count;
      return authorsEnvelope;
    }
  }
}
