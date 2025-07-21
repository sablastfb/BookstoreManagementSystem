using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreManagementSystem.WebApp.Features.Users;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsersController(IMediator mediator): Controller
{
  [HttpPost("login")]
  public Task<Login.UserEnvelope> Login([FromBody] Login.Command command,  CancellationToken cancellationToken)
    => mediator.Send(command, cancellationToken);
}
