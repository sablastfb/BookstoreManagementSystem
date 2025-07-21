using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Reviews;

public class Update
{
  public class Parameter
  {
    public short? Rating { get; init; } 
  }
  
  public record Command(long Id, Parameter Parameter) : IRequest<ReviewEnvelope>;

  public class Handler(BookstoreDbContext context) : IRequestHandler<Command, ReviewEnvelope>
  {
    public async Task<ReviewEnvelope> Handle(Command request, CancellationToken cancellationToken)
    {
      var review = await context.Reviews
        .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
      
      if (review == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Book = Constants.NOT_FOUND });
      }

      if (request.Parameter.Rating.HasValue)
      {
        review.Rating = request.Parameter.Rating.Value;
      }
      review.UpdatedAt = DateTime.UtcNow;
      context.Update(review);
      await context.SaveChangesAsync(cancellationToken);
      return  new ReviewEnvelope(review);
    }
  }
  
}
