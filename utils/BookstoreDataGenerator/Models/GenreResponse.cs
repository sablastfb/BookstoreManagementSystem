using System.Text.Json.Serialization;

namespace BookstoreDataGenerator.Models;


public class GenreResponse
{
  [JsonPropertyName("genres")]
  public List<GenreData> Genres { get; set; } = new List<GenreData>();
  
  [JsonPropertyName("count")]
  public int Count { get; set; }
   
  public class GenreData
  {
    [JsonPropertyName("id")]
    public short Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
  }
}
