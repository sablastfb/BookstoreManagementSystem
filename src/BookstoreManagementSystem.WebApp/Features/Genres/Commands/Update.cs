using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Genres.Commands;

public class Update
{
  public class Parameter
  {
    public string? Name { get; init; } 
  }
  
  public record Command(short Id, Parameter Parameter) : IRequest<GenreEnvelope>;
  
  public class GenreDataValidator : AbstractValidator<Parameter>
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
  
  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Parameter).NotNull().SetValidator(new GenreDataValidator());
    }
  }
  
  public class Handler(BookstoreDbContext context)
    : IRequestHandler<Command, GenreEnvelope>
  {
    public async Task<GenreEnvelope> Handle(
      Command command,
      CancellationToken cancellationToken
    )
    {
      var genre = await context.Genres
        .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);
      
      if (genre == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Genres = Constants.NOT_FOUND });
      }
      
      if (command.Parameter.Name != null)
      {
        genre.Name = command.Parameter.Name;
      }

      genre.UpdatedAt = DateTime.UtcNow;
      await context.SaveChangesAsync(cancellationToken);

      return new GenreEnvelope(genre);
    }
  }
}
