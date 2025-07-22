using Asp.Versioning;
using BookstoreManagementSystem.WebApp.Features.Authors;
using BookstoreManagementSystem.WebApp.Features.Books.Data;
using BookstoreManagementSystem.WebApp.Features.Books.Queries;
using BookstoreManagementSystem.WebApp.Features.Reviews;
using BookstoreManagementSystem.WebApp.Infrastructure.Secutiry;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Create = BookstoreManagementSystem.WebApp.Features.Books.Commands.Create;
using Delete = BookstoreManagementSystem.WebApp.Features.Books.Commands.Delete;
using Get = BookstoreManagementSystem.WebApp.Features.Books.Queries.Get;
using List = BookstoreManagementSystem.WebApp.Features.Books.Queries.List;
using Update = BookstoreManagementSystem.WebApp.Features.Books.Commands.Update;

namespace BookstoreManagementSystem.WebApp.Features.Books;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BooksController(IMediator mediator) : Controller
{
  /// <summary>
  /// Creates book
  /// </summary>
  [HttpPost]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<BookEnvelope> Create(
    [FromBody] Create.Command command,
    CancellationToken cancellationToken
  ) => mediator.Send(command, cancellationToken);

  /// <summary>
  /// Get book by id
  /// </summary>
  [HttpGet("{id}")]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<BookEnvelope> Get(Guid id, CancellationToken cancellationToken) => mediator.Send(new Get.Query(id), cancellationToken);

  /// <summary>
  /// Get list of books
  /// </summary>
  [HttpGet]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<BooksEnvelope> List(
    [FromQuery] int? limit,
    [FromQuery] int? offset,
    CancellationToken cancellationToken) => mediator.Send(new List.Query(limit, offset), cancellationToken);
  
  /// <summary>
  /// Update book
  /// </summary>
  [HttpPut]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<BookEnvelope> Update(  
    [FromBody] Update.Command command, CancellationToken cancellationToken) => mediator.Send(command, cancellationToken);
  
  /// <summary>
  /// Delete book
  /// </summary>
  [HttpDelete]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task Delete(Guid id,CancellationToken cancellationToken) => mediator.Send(new Delete.Command(id), cancellationToken);
  
  
  /// <summary>
  /// Return details of book, authors names, genres and average rating
  /// </summary>
  [HttpGet("details/{id}")]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<Deatails.BookDetailEnvelope> Details(Guid id, CancellationToken cancellationToken) => mediator.Send(new Deatails.Query(id), cancellationToken);
  
  
  /// <summary>
  /// Return list of  details of and sorts them by average rating, by defaults first 10
  /// </summary>
  [HttpGet("detailList")]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<BooksDetailsEnvelope> DetailsList(
    [FromQuery] int? limit,
    [FromQuery] int? offset,
    CancellationToken cancellationToken) => mediator.Send(new DetailsList.Query(limit, offset), cancellationToken);
}
