using System.Reflection;
using BookstoreManagementSystem.WebApp.Features.Authors;
using BookstoreManagementSystem.WebApp.Infrastructure;
using FluentValidation;
using MediatR;

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
  }
}
