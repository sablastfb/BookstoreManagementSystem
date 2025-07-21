using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Reviews;

public class Update
{
  public class Parameter
  {
    public string? Name { get; init; } 
  }
  
  public record Command(Guid Id, Parameter Parameter) : IRequest<ReviewEnvelope>;
}
