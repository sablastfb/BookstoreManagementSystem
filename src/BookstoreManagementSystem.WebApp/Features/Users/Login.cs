using FluentValidation;
using MediatR;

namespace BookstoreManagementSystem.WebApp.Features.Users;

public class Login
{
  public record UserData(string Username, string Password);
  public record UserEnvelope(string Token);
  
  public class UserDataValidator : AbstractValidator<UserData>
  {
    public UserDataValidator()
    {
      RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("Username is required.");
      RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password is required.");
    }
  }

  public record Command(UserData User) : IRequest<UserEnvelope>;

  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.User).NotNull().SetValidator(new UserDataValidator());
    }
  }

  public class Handler 
    : IRequestHandler<Command, UserEnvelope>
  {
    public Task<UserEnvelope> Handle(
      Command request,
      CancellationToken cancellationToken
    )
    {
      var userEnvelope = new UserEnvelope("234");
      return Task.FromResult(userEnvelope);
    }
  }
}
