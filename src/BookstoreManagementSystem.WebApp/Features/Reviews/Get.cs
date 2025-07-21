using BookstoreManagementSystem.WebApp.Features.Genres;
using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Reviews;

public class Get
{
  public record Query(Guid Id) : IRequest<ReviewEnvelope>;
  
}
