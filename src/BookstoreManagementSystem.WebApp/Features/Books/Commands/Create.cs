using BookstoreManagementSystem.WebApp.Domain;
using BookstoreManagementSystem.WebApp.Features.Books.Data;
using BookstoreManagementSystem.WebApp.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Books.Commands;

public class Create
{
  public class BookData
  {
    public required decimal Price { get; init; }
    public required string Title { get; init; }
    public Guid[]? AuthorList { get; init; }
    public short[]? GenreList { get; init; }
  }

  public record Command(BookData BookData) : IRequest<BookEnvelope>;

  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.BookData.Price)
        .NotNull()
        .WithMessage("Price is required")
        .GreaterThan(0)
        .WithMessage("Price must be positive");

      RuleFor(x => x.BookData.Title)
        .NotEmpty()
        .WithMessage("Book must have a title");

      RuleFor(x => x.BookData.AuthorList)
        .NotNull()
        .NotEmpty()
        .WithMessage("Book must have at least one author");

      RuleFor(x => x.BookData.GenreList)
        .NotNull()
        .NotEmpty()
        .WithMessage("Book must have at least one genre");
    }
  }

  public class Handler(BookstoreDbContext context, ILogger<Handler> logger)
    : IRequestHandler<Command, BookEnvelope>
  {
    public async Task<BookEnvelope> Handle(Command message, CancellationToken cancellationToken)
    {
      logger.LogInformation("Creating Book");
      var authors = new List<Author>();
      foreach (var authorId in (message.BookData.AuthorList ?? Enumerable.Empty<Guid>()))
      {
        var author = await context.Authors.FirstOrDefaultAsync(author => author.Id == authorId, cancellationToken);

        if (author == null)
        {
          logger.LogError("Author not found {}.", authorId);
          throw new Exception($"Author with ID={authorId} not found");
        }

        authors.Add(author);
      }

      var genres = new List<Genre>();
      foreach (var genreId in (message.BookData.GenreList ?? Enumerable.Empty<short>()))
      {
        var genre = await context.Genres.FirstOrDefaultAsync(genre => genre.Id == genreId, cancellationToken);
        if (genre == null)
        {
          logger.LogError("Genre not found {}.", genreId);
          throw new Exception($"Genre with ID={genreId} not found");
        }

        genres.Add(genre);
      }

      var book = new Book
      {
        Price = message.BookData.Price,
        Title = message.BookData.Title, 
        CreatedAt = DateTime.UtcNow, 
        UpdatedAt = DateTime.UtcNow,
      };

      await context.Books.AddAsync(book, cancellationToken);

      await context.BookGenres.AddRangeAsync(
        genres.Select(g => new BookGenre { Book = book, Genre = g }),
        cancellationToken
      );

      await context.BookAuthors.AddRangeAsync(
        authors.Select(a => new BookAuthor { Book = book, Author = a }),
        cancellationToken
      );

      await context.SaveChangesAsync(cancellationToken);
      logger.LogInformation("Book created");
      return new BookEnvelope(book);
    }
  }
}
