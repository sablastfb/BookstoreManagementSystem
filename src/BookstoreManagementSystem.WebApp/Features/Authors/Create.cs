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
      RuleFor(x => x.Name).NotNull().NotEmpty();
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
