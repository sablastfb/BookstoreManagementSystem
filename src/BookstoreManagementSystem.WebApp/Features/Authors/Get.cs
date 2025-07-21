using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public class Get
{
  public record Query(Guid id) : IRequest<AuthorEnvelope>;
}
