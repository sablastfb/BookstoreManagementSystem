using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Reviews;

public class List
{
  public record Query() : IRequest<ReviewsEnvelope>;

}
