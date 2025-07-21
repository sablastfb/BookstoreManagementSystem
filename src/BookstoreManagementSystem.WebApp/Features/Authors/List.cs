using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public class List
{
  public record Query() : IRequest<AuthorsEnvelope>;
}
