using BookstoreManagementSystem.WebApp.Domain;
using BookstoreManagementSystem.WebApp.Infrastructure;
using FluentValidation;
using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public class Create
{
  public class AuthorData
  {
    public string? Name { get; init; }
    public int BirthYear { get; init; }
  }

  public class AuthorDataValidator : AbstractValidator<AuthorData>
  {
    public AuthorDataValidator()
    {
      RuleFor(x => x.BirthYear)
        .GreaterThan(0)
        .WithMessage("Birth year must be greater than 0")
        .LessThanOrEqualTo(DateTime.Now.Year)
        .WithMessage("Birth year cannot be in the future")
        .GreaterThanOrEqualTo(1000)
        .WithMessage("Birth year must be realistic (after year 1000)");
      
      RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Author name is required")
        .Length(1, 200)
        .WithMessage("Author name must be between 1 and 200 characters")
        .Matches(@"^[a-zA-Z\s\-\.\']+$")
        .WithMessage("Author name can only contain letters, spaces, hyphens, periods, and apostrophes");
    }
  }

  public record Command(AuthorData Author) : IRequest<AuthorEnvelope>;

  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Author).NotNull().SetValidator(new AuthorDataValidator());
    }
  }

  public class Handler(BookstoreDbContext context)
    : IRequestHandler<Command, AuthorEnvelope>
  {
    public async Task<AuthorEnvelope> Handle(
      Command message,
      CancellationToken cancellationToken
    )
    {
      var author = new Author
      {
        Name = message.Author.Name,
        BirthYear = message.Author.BirthYear,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
      };

      await context.Authors.AddAsync(author, cancellationToken);
      await context.SaveChangesAsync(cancellationToken);
      return new AuthorEnvelope(author);
    }
  }
}
