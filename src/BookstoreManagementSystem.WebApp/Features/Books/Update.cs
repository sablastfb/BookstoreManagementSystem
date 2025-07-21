using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class Update
{
  
  public class Parameter
  {
    public decimal? Price { get; init; } 
  }
  
  public record Command(Guid Id,Parameter Parameter) : IRequest<BookEnvelope>;

}
