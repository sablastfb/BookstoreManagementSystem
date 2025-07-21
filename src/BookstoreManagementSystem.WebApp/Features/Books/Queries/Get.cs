using System.Net;
using BookstoreManagementSystem.WebApp.Features.Books.Data;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Books.Queries;

public class Get
{
  public record Query(Guid Id) : IRequest<BookEnvelope>;
  
  public class Handler(BookstoreDbContext context) : IRequestHandler<Query, BookEnvelope>
  {
    public async Task<BookEnvelope> Handle(Query request, CancellationToken cancellationToken)
    {
      var book = await context
        .Books
        .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
      
      if (book == null)
      {
        throw new RestException(
          HttpStatusCode.NotFound,
          new { Book = Constants.NOT_FOUND }
        );
      }
      return new BookEnvelope(book);
    }
  }
}
