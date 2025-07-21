using Asp.Versioning;
using BookstoreManagementSystem.WebApp.Features.Authors;
using BookstoreManagementSystem.WebApp.Features.Books.Data;
using BookstoreManagementSystem.WebApp.Features.Reviews;
using BookstoreManagementSystem.WebApp.Infrastructure.Secutiry;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreManagementSystem.WebApp.Features.Books;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BooksController(IMediator mediator) : Controller
{
  [HttpPost]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<BookEnvelope> Create(
    [FromBody] Create.Command command,
    CancellationToken cancellationToken
  ) => mediator.Send(command, cancellationToken);


  [HttpGet("{id}")]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<BookEnvelope> Get(Guid id, CancellationToken cancellationToken) => mediator.Send(new Get.Query(id), cancellationToken);

  [HttpGet]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<BooksEnvelope> List(
    [FromQuery] int? limit,
    [FromQuery] int? offset,
    CancellationToken cancellationToken) => mediator.Send(new List.Query(limit, offset), cancellationToken);
  
  [HttpPut]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<BookEnvelope> Update(  
    [FromBody] Update.Command command, CancellationToken cancellationToken) => mediator.Send(command, cancellationToken);
  
  [HttpDelete]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task Delete(Guid id,CancellationToken cancellationToken) => mediator.Send(new Delete.Command(id), cancellationToken);
  
  [HttpGet("details/{id}")]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<Deatails.BookDetailEnvelope> Details(Guid id, CancellationToken cancellationToken) => mediator.Send(new Deatails.Query(id), cancellationToken);
  
  [HttpGet("detailList")]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<BooksDetailsEnvelope> DetailsList(
    [FromQuery] int? limit,
    [FromQuery] int? offset,
    CancellationToken cancellationToken) => mediator.Send(new DetailsList.Query(limit, offset), cancellationToken);
}
