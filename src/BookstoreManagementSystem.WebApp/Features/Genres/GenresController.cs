using Asp.Versioning;
using BookstoreManagementSystem.WebApp.Features.Genres.Commands;
using BookstoreManagementSystem.WebApp.Features.Genres.Queries;
using BookstoreManagementSystem.WebApp.Infrastructure.Secutiry;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreManagementSystem.WebApp.Features.Genres;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class GenresController(IMediator mediator) : Controller
{
  /// <summary>
  /// Create genre
  /// </summary>
  [HttpPost]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<GenreEnvelope> Create(
    [FromBody] Create.Command command,
    CancellationToken cancellationToken
  ) => mediator.Send(command, cancellationToken);
  
  /// <summary>
  /// Get genre by id
  /// </summary>
  [HttpGet("{id}")]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<GenreEnvelope> Get(short id, CancellationToken cancellationToken) => mediator.Send(new Get.Query(id), cancellationToken);
  
  /// <summary>
  /// Get list
  /// </summary>
  [HttpGet]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<GenresEnvelope> List(CancellationToken cancellationToken) => mediator.Send(new List.Query(), cancellationToken);
  
  /// <summary>
  /// Update genre
  /// </summary>
  [HttpPut]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<GenreEnvelope> Update(  
    [FromBody] Update.Command command, CancellationToken cancellationToken) => mediator.Send(command, cancellationToken);
  
  /// <summary>
  /// Delete genre
  /// </summary>
  [HttpDelete]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task Delete(short id,CancellationToken cancellationToken) => mediator.Send(new Delete.Command(id), cancellationToken);
}
