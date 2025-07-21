using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Genres.Commands;

public class Delete
{
  public record Command(short Id) : IRequest;
  
  public class Handler(BookstoreDbContext context) : IRequestHandler<Command>
  {
    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
      var genre = await context.Genres
        .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

      if (genre == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Genres = Constants.NOT_FOUND });
      }      

      var hasBooks = await context.Books
        .AnyAsync(b => b.BookGenres.Any(bg => bg.GenreId == request.Id), cancellationToken);

      if (hasBooks){
        throw new RestException( HttpStatusCode.BadRequest, new { Book = Constants.IN_USE });
      }

      context.Genres.Remove(genre);
      await context.SaveChangesAsync(cancellationToken);
    }
  }

}
