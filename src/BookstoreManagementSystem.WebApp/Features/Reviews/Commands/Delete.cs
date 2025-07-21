using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Reviews.Commands;

public class Delete
{
  public record Command(long Id) : IRequest;
  public class Handler(BookstoreDbContext context) : IRequestHandler<Command>
  {
    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
      var author = await context.Reviews
        .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
      
      if (author == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Review = Constants.NOT_FOUND });
      }      

      context.Reviews.Remove(author);
      await context.SaveChangesAsync(cancellationToken);
    }
  }
}
