using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public class Update
{
  public class Parameter
  {
    public string? Name { get; init; } 
    public int? BirthYear { get; init; } 
  }
  
  public record Command(Guid Id, Parameter Parameter) : IRequest<AuthorEnvelope>;
  
  public class AuthorUpdateDataValidator : AbstractValidator<Update.Parameter>
  {
    public AuthorUpdateDataValidator()
    {
      RuleFor(x => x.BirthYear)
        .GreaterThan(0)
        .WithMessage("Birth year must be greater than 0")
        .LessThanOrEqualTo(DateTime.Now.Year)
        .WithMessage("Birth year cannot be in the future")
        .GreaterThanOrEqualTo(1000)
        .WithMessage("Birth year must be realistic (after year 1000)");
      
      RuleFor(x => x.Name)
        .Length(1, 200)
        .WithMessage("Author name must be between 1 and 200 characters")
        .Matches(@"^[a-zA-Z\s\-\.\']+$")
        .WithMessage("Author name can only contain letters, spaces, hyphens, periods, and apostrophes");
    }
  }

  public class CommandValidator : AbstractValidator<Update.Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
      RuleFor(x => x.Parameter).NotNull().SetValidator(new Update.AuthorUpdateDataValidator());
    }
  }
  
  public class Handler(BookstoreDbContext context): IRequestHandler<Command, AuthorEnvelope>
  {
    public async Task<AuthorEnvelope> Handle(Command command, CancellationToken cancellationToken)
    {
      var author = await context.Authors
        .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);
      
      if (author == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
      }
      
      if (command.Parameter.Name != null)
      {
        author.Name = command.Parameter.Name;
      }

      if (command.Parameter.BirthYear.HasValue)
      {
        author.BirthYear = command.Parameter.BirthYear.Value;
      }

      author.UpdatedAt = DateTime.UtcNow;
      await context.SaveChangesAsync(cancellationToken);

      return new AuthorEnvelope(author);
    }
  }
}
