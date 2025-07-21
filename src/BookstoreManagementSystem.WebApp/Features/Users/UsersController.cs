using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreManagementSystem.WebApp.Features.Users;

[Route("users")]
public class UsersController(IMediator mediator): Controller
{
  [HttpPost("login")]
  public Task<Login.UserEnvelope> Login([FromBody] Login.Command command,  CancellationToken cancellationToken)
    => mediator.Send(command, cancellationToken);
}
