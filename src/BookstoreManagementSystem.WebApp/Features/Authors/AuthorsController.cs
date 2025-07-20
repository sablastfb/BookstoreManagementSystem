using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

[Route("authors")]
public class AuthorsController(IMediator mediator) : Controller
{
  [HttpPost]
  public Task<AuthorEnvelope> Create(
    [FromBody] Create.Command command,
    CancellationToken cancellationToken
  ) => mediator.Send(command, cancellationToken);
}
