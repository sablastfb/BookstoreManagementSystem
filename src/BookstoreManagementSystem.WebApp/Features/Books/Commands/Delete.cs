using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Books.Commands;

public class Delete
{
  public record Command(Guid Id) : IRequest;
  
  public class Handler(BookstoreDbContext context) : IRequestHandler<Command>
  {
    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
      var book = await context.Books
        .FirstOrDefaultAsync(b => b.Id == request.Id , cancellationToken);

      if (book == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Book = Constants.NOT_FOUND });
      }      
      
      context.Books.Remove(book);
      await context.SaveChangesAsync(cancellationToken);
    }
  }
}
