using System.Reflection;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using BookstoreManagementSystem.WebApp.Features.Authors;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Secutiry;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace BookstoreManagementSystem.WebApp;

public static class ServicesExtensions
{
  public static void AddBookstoreManagementSystem(this IServiceCollection services)
  {
    services.AddMediatR(cfg =>
      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
    );
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    services.AddValidatorsFromAssemblyContaining<Create.CommandValidator>();
    services.AddApiVersioning(options =>
      {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
          new UrlSegmentApiVersionReader(),
          new HeaderApiVersionReader("X-Api-Version"));
      })
      .AddMvc()
      .AddApiExplorer(options =>
      {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
      });
  }

public static void AddJWT(this IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = configuration["Jwt:Issuer"],
              ValidAudience = configuration["Jwt:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty)),
                
              NameClaimType = ClaimTypes.Name,
              RoleClaimType = ClaimTypes.Role,
              ClockSkew = TimeSpan.Zero
            };
            
            options.Events = new JwtBearerEvents
            {
              OnAuthenticationFailed = context =>
              {
                Console.WriteLine($"Authentication failed: {context.Exception}");
                return Task.CompletedTask;
              }
            };
        });

    services.AddAuthorization(options =>
    {
        options.AddPolicy(JwtIssuerOptions.Reader, policy => 
            policy.RequireRole(JwtIssuerOptions.Reader));
        options.AddPolicy(JwtIssuerOptions.Admin, policy => 
            policy.RequireRole(JwtIssuerOptions.Admin));
    });
}
  public static void AddSerilogLogging(this ILoggerFactory loggerFactory)
  {
    var log = new LoggerConfiguration()
      .MinimumLevel.Verbose()
      .Enrich.FromLogContext()
      .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}",
        theme: AnsiConsoleTheme.Code
      )
      .CreateLogger();

    loggerFactory.AddSerilog(log);
    Log.Logger = log;
  }
}
