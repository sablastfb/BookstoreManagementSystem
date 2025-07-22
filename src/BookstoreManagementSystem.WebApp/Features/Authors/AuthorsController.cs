using Asp.Versioning;
using BookstoreManagementSystem.WebApp.Features.Authors.Commands;
using BookstoreManagementSystem.WebApp.Features.Authors.Data;
using BookstoreManagementSystem.WebApp.Features.Authors.Queries;
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
  /// <summary>
  /// Creates new Author
  /// </summary>
  [HttpPost]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<AuthorEnvelope> Create(
    [FromBody] Create.Command command,
    CancellationToken cancellationToken
  ) => mediator.Send(command, cancellationToken);

  
  /// <summary>
  /// Returns author by id 
  /// </summary>
  [HttpGet("{id}")]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<AuthorEnvelope> Get(Guid id, CancellationToken cancellationToken) =>
    mediator.Send(new Get.Query(id), cancellationToken);

  
  /// <summary>
  /// Returns list of authors 
  /// </summary>
  [HttpGet]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<AuthorsEnvelope> List(
    [FromQuery] int? limit,
    [FromQuery] int? offset,
    CancellationToken cancellationToken) 
    => mediator.Send(new List.Query(limit, offset), cancellationToken);

  /// <summary>
  /// Updates author
  /// </summary>
  [HttpPut]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<AuthorEnvelope> Update(
    [FromBody] Update.Command command, CancellationToken cancellationToken) =>
    mediator.Send(command, cancellationToken);

  /// <summary>
  /// Delete author
  /// </summary>
  [HttpDelete]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task Delete(Guid id, CancellationToken cancellationToken) =>
    mediator.Send(new Delete.Command(id), cancellationToken);
}
