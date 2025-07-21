using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public class Update
{
  public class Model
  {
    public Guid Id { get; init; }
    public string? Name { get; init; } 
    public int? BirthYear { get; init; } 
  }
  
  public record Command(Model Model) : IRequest<AuthorEnvelope>;
  
  
}
