using BookstoreManagementSystem.WebApp.Domain;
using BookstoreManagementSystem.WebApp.Infrastructure;
using FluentValidation;
using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public class Create
{
     public class AuthorData
    {
    }

    public class ArticleDataValidator : AbstractValidator<AuthorData>
    {
        public ArticleDataValidator()
        {
        }
    }

    public record Command(AuthorData Article) : IRequest<ArticleEnvelope>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() =>
            RuleFor(x => x.Article).NotNull().SetValidator(new ArticleDataValidator());
    }

    public class Handler(BookstoreDbContext context)
        : IRequestHandler<Command, ArticleEnvelope>
    {
        public async Task<ArticleEnvelope> Handle(
            Command message,
            CancellationToken cancellationToken
        )
        {
            await context.Authors.AddAsync(new Author());
            return new ArticleEnvelope();
        }
    }
}
