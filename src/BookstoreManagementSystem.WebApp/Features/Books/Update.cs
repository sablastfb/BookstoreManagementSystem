using System.Net;
using BookstoreManagementSystem.WebApp.Features.Books.Data;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class Update
{
  
  public class Parameter
  {
    public decimal? Price { get; init; } 
  }
  
  public record Command(Guid Id,Parameter Parameter) : IRequest<BookEnvelope>;

  
  public class AuthorUpdateDataValidator : AbstractValidator<Parameter>
  {
    public AuthorUpdateDataValidator()
    {
      RuleFor(x => x.Price)
        .NotNull()
        .WithMessage("Price is required")
        .GreaterThan(0)
        .WithMessage("Price must be positive");
    }
  }
  
  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
      RuleFor(x => x.Parameter).NotNull().SetValidator(new Update.AuthorUpdateDataValidator());
    }
  }
  
  
  public class Handler(BookstoreDbContext context): IRequestHandler<Command, BookEnvelope>
  {
    public async Task<BookEnvelope> Handle(Command message, CancellationToken cancellationToken)
    {
      var book = await context.Books
        .FirstOrDefaultAsync(a => a.Id == message.Id, cancellationToken);
      
      if (book == null) {
        throw new RestException( HttpStatusCode.NotFound, new { Book = Constants.NOT_FOUND });
      }

      if (message.Parameter.Price.HasValue)
      {
        book.Price = message.Parameter.Price.Value;
      }
      book.UpdatedAt = DateTime.UtcNow;
      context.Update(book);
      await context.SaveChangesAsync(cancellationToken);
      return  new BookEnvelope(book);
    }
  }
}
