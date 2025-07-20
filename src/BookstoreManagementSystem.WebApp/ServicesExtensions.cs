using System.Reflection;

namespace BookstoreManagementSystem.WebApp;

public static class ServicesExtensions
{
  public static void AddBookstoreManagementSystem(this IServiceCollection services)
  {
    services.AddMediatR(cfg =>
      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
    );
  }
}
