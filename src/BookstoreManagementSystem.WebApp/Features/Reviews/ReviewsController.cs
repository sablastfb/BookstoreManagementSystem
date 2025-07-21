using Asp.Versioning;
using BookstoreManagementSystem.WebApp.Features.Reviews.Commands;
using BookstoreManagementSystem.WebApp.Features.Reviews.Data;
using BookstoreManagementSystem.WebApp.Infrastructure.Secutiry;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreManagementSystem.WebApp.Features.Reviews;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ReviewsController(IMediator mediator) : Controller
{
  [HttpPost]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<ReviewEnvelope> Create(
    [FromBody] Create.Command command,
    CancellationToken cancellationToken
  ) => mediator.Send(command, cancellationToken);
  
  [HttpPut]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task<ReviewEnvelope> Update(  
    [FromBody] Update.Command command, CancellationToken cancellationToken) => mediator.Send(command, cancellationToken);
  
  [HttpDelete]
  [Authorize(Roles = JwtIssuerOptions.Admin)]
  public Task Delete(long id,CancellationToken cancellationToken) => mediator.Send(new Delete.Command(id), cancellationToken);
}
