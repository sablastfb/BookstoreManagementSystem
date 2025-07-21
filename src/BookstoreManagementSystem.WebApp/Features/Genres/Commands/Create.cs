using BookstoreManagementSystem.WebApp.Domain;
using BookstoreManagementSystem.WebApp.Infrastructure;
using FluentValidation;
using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Genres;

public class Create
{
  public class GenreData
  {
    public required string Name { get; init; }
  }
  
  public class GenreDataValidator : AbstractValidator<GenreData>
  {
    public GenreDataValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Genre name is required")
        .Length(1, 50)
        .WithMessage("Genre name must be between 1 and 50 characters");
    }
  }
  
  public record Command(GenreData Genre) : IRequest<GenreEnvelope>;
  
  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Genre).NotNull().SetValidator(new GenreDataValidator());
    }
  }
  
  public class Handler(BookstoreDbContext context)
    : IRequestHandler<Command, GenreEnvelope>
  {
    public async Task<GenreEnvelope> Handle(
      Command message,
      CancellationToken cancellationToken
    )
    {
      var genre = new Genre
      {
        Name = message.Genre.Name,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
      };

      await context.Genres.AddAsync(genre, cancellationToken);
      await context.SaveChangesAsync(cancellationToken);
      return new GenreEnvelope(genre);
    }
  }
}
