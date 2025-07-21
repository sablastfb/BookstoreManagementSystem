using BookstoreManagementSystem.WebApp.Features.Books.Data;
using BookstoreManagementSystem.WebApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BookstoreManagementSystem.WebApp.Features.Books.Queries;

public class DetailsList
{
  public record BookDetailDto(
    string Title,
    decimal Price,
    double AverageRating,
    string AuthorNames,
    string GenreNames);
  
  public record Query(
    int? Limit,
    int? Offset) : IRequest<BooksDetailsEnvelope>;

  public class Handler(BookstoreDbContext context) : IRequestHandler<Query, BooksDetailsEnvelope>
  {
    public async Task<BooksDetailsEnvelope> Handle(Query request, CancellationToken cancellationToken)
    {
      var sql = ResourceLoader.GetSqlQuery("Books", "DetailsList");

      var booksDetailsDto = await context.Database
        .SqlQueryRaw<BookDetailDto>(sql
          ,
          new NpgsqlParameter("Limit", request.Limit ?? 10),
          new NpgsqlParameter("Offset", request.Offset ?? 0)
        )
        .ToListAsync(cancellationToken);
      var booksDetails = BookDetailMapper.MapToEnvelope(booksDetailsDto);
      booksDetails.Count = context.Books.Count();
      
      return booksDetails;
    }
  }
}
