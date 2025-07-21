using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Authors.Commands
{
  public class Delete
  {
    public record Command(Guid Id) : IRequest;

    public class Handler(BookstoreDbContext context) : IRequestHandler<Command>
    {
      public async Task Handle(Command request, CancellationToken cancellationToken)
      {
        var author = await context.Authors
          .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (author == null) {
          throw new RestException( HttpStatusCode.NotFound, new { Authors = Constants.NOT_FOUND });
        }      

        var hasBooks = await context.Books
          .AnyAsync(b => b.BookAuthors.Any(a => a.AuthorId == request.Id), cancellationToken);

        if (hasBooks){
          throw new RestException( HttpStatusCode.BadRequest, new { Book = Constants.IN_USE });
        }

        context.Authors.Remove(author);
        await context.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
