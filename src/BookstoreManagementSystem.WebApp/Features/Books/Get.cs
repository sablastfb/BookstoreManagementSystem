using BookstoreManagementSystem.WebApp.Features.Authors;
using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class Get
{
  public record Query(Guid Id) : IRequest<BookEnvelope>;

}
