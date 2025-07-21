using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Genres;

public class Delete
{
  public record Command(short Id) : IRequest;
  
  public class Handler(BookstoreDbContext context) : IRequestHandler<Command>
  {
    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
      var genre = await context.Genres
        .FindAsync(new object[] { request.Id }, cancellationToken);

      if (genre == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
      }      

      var hasBooks = await context.Books
        .AnyAsync(b => b.BookGenres.Any(b => b.GenreId == request.Id), cancellationToken);

      if (hasBooks){
        throw new RestException( HttpStatusCode.BadRequest, new { Article = Constants.IN_USE });
      }

      context.Genres.Remove(genre);
      await context.SaveChangesAsync(cancellationToken);
    }
  }

}
