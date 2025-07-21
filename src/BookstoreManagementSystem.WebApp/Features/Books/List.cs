using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class List
{
    public record Query() : IRequest<BooksEnvelope>;

}
