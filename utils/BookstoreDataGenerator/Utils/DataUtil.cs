using System.Text;
using System.Text.Json;

namespace BookstoreDataGenerator.Utils;

public static class DataUtil
{
  public static StringContent  JsonContentHelper(object obj)
  {
    return new StringContent(
      JsonSerializer.Serialize(obj,
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
      Encoding.UTF8,
      "application/json");
  }
}
