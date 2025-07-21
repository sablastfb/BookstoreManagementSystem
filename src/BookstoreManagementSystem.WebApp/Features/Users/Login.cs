using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookstoreManagementSystem.WebApp.Infrastructure.Secutiry;
using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace BookstoreManagementSystem.WebApp.Features.Users;

public class Login
{
  public record UserData(string UserName, string Password);

  public record UserEnvelope(string Token);

  public class UserDataValidator : AbstractValidator<UserData>
  {
    public UserDataValidator()
    {
      RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("Username is required.");
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

  public class Handler(IConfiguration configuration)
    : IRequestHandler<Command, UserEnvelope>
  {
    public Task<UserEnvelope> Handle(
      Command message,
      CancellationToken cancellationToken
    )
    {
      var user = message.User.UserName switch
      {
        "reader" => new { Role = JwtIssuerOptions.Reader },
        "admin" => new { Role = JwtIssuerOptions.Admin },
        _ => null
      };
      if (user == null) throw new ArgumentNullException(nameof(user));

      var token = GenerateJwtToken(message.User.UserName, user.Role);
      var userEnvelope = new UserEnvelope(token);
      return Task.FromResult(userEnvelope);
    }

    private string GenerateJwtToken(string username, string role)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Role, role),
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat,
          new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
          ClaimValueTypes.Integer64)
      };

      var token = new JwtSecurityToken(
        issuer: configuration["Jwt:Issuer"],
        audience: configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpireMinutes"])), 
        signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
