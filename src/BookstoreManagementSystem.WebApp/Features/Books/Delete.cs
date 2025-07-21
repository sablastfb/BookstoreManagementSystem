using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class Delete
{
  public record Command(Guid Author) : IRequest;
}
