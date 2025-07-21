using BookstoreManagementSystem.WebApp.Infrastructure.Secutiry;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreManagementSystem.WebApp.Features.Books;

[Route("books")]
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
  public Task<BooksEnvelope> List(CancellationToken cancellationToken) => mediator.Send(new List.Query(), cancellationToken);
  
  [HttpPut]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<BookEnvelope> Update(  
    [FromBody] Update.Command command, CancellationToken cancellationToken) => mediator.Send(command, cancellationToken);
  
  [HttpDelete]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task Delete(Guid id,CancellationToken cancellationToken) => mediator.Send(new Delete.Command(id), cancellationToken);
  
}
