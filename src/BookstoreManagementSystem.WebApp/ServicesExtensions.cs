using System.Reflection;
using BookstoreManagementSystem.WebApp.Features.Authors;
using BookstoreManagementSystem.WebApp.Infrastructure;
using FluentValidation;
using MediatR;
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
