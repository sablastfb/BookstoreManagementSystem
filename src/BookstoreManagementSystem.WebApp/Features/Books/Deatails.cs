using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class Deatails
{
  public record Query(Guid Id) : IRequest<BooksEnvelope>;

}
