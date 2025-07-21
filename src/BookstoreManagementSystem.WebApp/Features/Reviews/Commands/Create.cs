using System.Net;
using BookstoreManagementSystem.WebApp.Domain;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Reviews;

public class Create
{
  public class ReviewData
  {
    public Guid BookId { get; init; }
    public short Rating { get; init; }
  }

  public record Command(ReviewData Review) : IRequest<ReviewEnvelope>;
  public class AuthorDataValidator : AbstractValidator<ReviewData>
  {
    public AuthorDataValidator()
    {
      RuleFor(x => x.Rating)
        .NotNull()
        .WithMessage("Rating is required")
        .InclusiveBetween((short)1, (short)5)
        .WithMessage("Rating must be between 1 and 5");
    }
  }


  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Review)
        .NotNull()
        .SetValidator(new AuthorDataValidator());
    }
  }
  
  public class Handler(BookstoreDbContext context)
    : IRequestHandler<Command, ReviewEnvelope>
  {
    public async Task<ReviewEnvelope> Handle(
      Command message,
      CancellationToken cancellationToken
    )
    {
      var book = await context
        .Books.AsNoTracking()
        .FirstOrDefaultAsync(b => b.Id == message.Review.BookId, cancellationToken);
      
      if (book == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
      }
      
      var review = new Review
      {
        BookId = book.Id,
        Rating = message.Review.Rating,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
      };

      await context.Reviews.AddAsync(review);
      await context.SaveChangesAsync(cancellationToken);
      return new ReviewEnvelope(review);
    }
  }
}
