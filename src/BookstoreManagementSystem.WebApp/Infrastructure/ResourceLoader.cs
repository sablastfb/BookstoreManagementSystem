using System.Reflection;

namespace BookstoreManagementSystem.WebApp.Infrastructure;

public class ResourceLoader
{
  public static string GetSqlQuery(string feature, string queryName)
  {
    var assembly = Assembly.GetExecutingAssembly();
    var resourceName = $"{assembly.GetName().Name}.Features.{feature}.Queries.Sql.{queryName}.sql";
    using var stream = assembly.GetManifestResourceStream(resourceName);

    if (stream == null) throw new FileNotFoundException($"SQL resource {resourceName} not found");
        
    using var reader = new StreamReader(stream);
    return reader.ReadToEnd();
  }
}
