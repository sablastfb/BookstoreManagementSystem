using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Reviews;

public class Delete
{
  public record Command(Guid Id) : IRequest;

}
