using System.Text.Json.Serialization;

namespace BookstoreManagementSystem.Scheduler.Models;

public class Book
{
  [JsonPropertyName("price")]
  public decimal Price { get; set; }

  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;
}

