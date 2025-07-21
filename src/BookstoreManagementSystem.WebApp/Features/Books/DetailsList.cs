using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class DetailsList
{
  public record Query() : IRequest<BooksEnvelope>;
}
