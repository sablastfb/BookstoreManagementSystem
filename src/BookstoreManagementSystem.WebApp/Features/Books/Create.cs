using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class Create
{
  public class BookData
  {
    public required string Name { get; init; }
    public required int BirthYear { get; init; }
  }
  public record Command(BookData Author) : IRequest<BookEnvelope>;

}
