using Asp.Versioning;
using BookstoreManagementSystem.WebApp.Infrastructure.Secutiry;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthorsController(IMediator mediator) : Controller
{
  [HttpPost]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<AuthorEnvelope> Create(
    [FromBody] Create.Command command,
    CancellationToken cancellationToken
  ) => mediator.Send(command, cancellationToken);

  [HttpGet("{id}")]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<AuthorEnvelope> Get(Guid id, CancellationToken cancellationToken) =>
    mediator.Send(new Get.Query(id), cancellationToken);

  [HttpGet]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<AuthorsEnvelope> List(
    [FromQuery] int? limit,
    [FromQuery] int? offset,
    CancellationToken cancellationToken) 
    => mediator.Send(new List.Query(limit, offset), cancellationToken);

  [HttpPut]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<AuthorEnvelope> Update(
    [FromBody] Update.Command command, CancellationToken cancellationToken) =>
    mediator.Send(command, cancellationToken);

  
  [HttpDelete]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task Delete(Guid id, CancellationToken cancellationToken) =>
    mediator.Send(new Delete.Command(id), cancellationToken);
}
