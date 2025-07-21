using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public class Get
{
  public record Query(Guid Id) : IRequest<AuthorEnvelope>;

  public class Handler(BookstoreDbContext context) : IRequestHandler<Query,  AuthorEnvelope>
  {
    public async Task<AuthorEnvelope> Handle(Query request, CancellationToken cancellationToken)
    {
      var author = await context.Authors
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

      if (author == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
      }      
      
      return new AuthorEnvelope(author);
    }
  }
}
