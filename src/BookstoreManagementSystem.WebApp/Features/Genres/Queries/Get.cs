using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Genres.Queries;

public class Get
{
  public record Query(short Id) : IRequest<GenreEnvelope>;

  public class Handler(BookstoreDbContext context) : IRequestHandler<Query, GenreEnvelope>
  {
    public async Task<GenreEnvelope> Handle(Query request, CancellationToken cancellationToken)
    {
      var genre = await context.Genres
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

      if (genre == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Genres = Constants.NOT_FOUND });
      }      
      
      return new GenreEnvelope(genre);
    }
  }
}
