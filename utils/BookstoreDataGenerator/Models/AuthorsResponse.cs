using System.Text.Json.Serialization;

namespace BookstoreDataGenerator.Models;

public class AuthorsResponse
{
  [JsonPropertyName("authors")]
  public List<AuthorData> Authors { get; set; } = new List<AuthorData>();

  [JsonPropertyName("count")]
  public int Count { get; set; }
  
  public class AuthorData
  {
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("birthYear")]
    public int BirthYear { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
  }
}
