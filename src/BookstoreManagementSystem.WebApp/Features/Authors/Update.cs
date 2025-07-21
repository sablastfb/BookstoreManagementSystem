using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public class Update
{
  public class Model
  {
    public required Guid Id { get; init; }
    public string? Name { get; init; } 
    public int? BirthYear { get; init; } 
  }
  
  public record Command(Model Model) : IRequest<AuthorEnvelope>;
  
  public class Handler(BookstoreDbContext context): IRequestHandler<Command, AuthorEnvelope>
  {
    public async Task<AuthorEnvelope> Handle(Command command, CancellationToken cancellationToken)
    {
      var author = await context.Authors
        .FirstOrDefaultAsync(a => a.Id == command.Model.Id, cancellationToken);
      
      if (author == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
      }
      
      if (command.Model.Name != null)
      {
        author.Name = command.Model.Name;
      }

      if (command.Model.BirthYear.HasValue)
      {
        author.BirthYear = command.Model.BirthYear.Value;
      }

      author.UpdatedAt = DateTime.UtcNow;
      await context.SaveChangesAsync(cancellationToken);

      return new AuthorEnvelope(author);
    }
  }
}
