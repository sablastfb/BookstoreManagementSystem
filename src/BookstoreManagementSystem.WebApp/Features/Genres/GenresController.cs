using Asp.Versioning;
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
  [HttpPost]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<GenreEnvelope> Create(
    [FromBody] Create.Command command,
    CancellationToken cancellationToken
  ) => mediator.Send(command, cancellationToken);
  
  
  [HttpGet("{id}")]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<GenreEnvelope> Get(short id, CancellationToken cancellationToken) => mediator.Send(new Get.Query(id), cancellationToken);
  
  [HttpGet]
  [Authorize(Roles = $"{JwtIssuerOptions.Admin},{JwtIssuerOptions.Reader}")]
  public Task<GenresEnvelope> List(CancellationToken cancellationToken) => mediator.Send(new List.Query(), cancellationToken);
  
  
  [HttpPut]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<GenreEnvelope> Update(  
    [FromBody] Update.Command command, CancellationToken cancellationToken) => mediator.Send(command, cancellationToken);
  
  [HttpDelete]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task Delete(short id,CancellationToken cancellationToken) => mediator.Send(new Delete.Command(id), cancellationToken);
}
