using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BookstoreDataGenerator.Models;

public class BooksResponse
{
  [JsonPropertyName("books")]
  public List<BookDto> Books { get; set; } = new List<BookDto>();

  [JsonPropertyName("count")]
  public int Count { get; set; }
}

public class BookDto
{
  [JsonPropertyName("id")]
  public Guid Id { get; set; }
  
  [JsonPropertyName("price")]
  public decimal Price { get; set; }

  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;

  [JsonPropertyName("createdAt")]
  public DateTime CreatedAt { get; set; }

  [JsonPropertyName("updatedAt")]
  public DateTime UpdatedAt { get; set; }
}
