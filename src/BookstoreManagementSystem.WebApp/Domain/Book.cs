using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookstoreManagementSystem.WebApp.Domain;

public class Book
{
  public Guid Id { get; set; } 
  
  [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
  public decimal Price { get; set; }
  
  [MinLength(1), MaxLength(200)]
  public required string Title { get; init; }
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }

  [JsonIgnore]
  public List<BookAuthor> BookAuthors { get; set; } = new ();
  
  [JsonIgnore]
  public List<BookGenre> BookGenres { get; set; } = new ();

  [JsonIgnore]
  public List<Review> Reviews { get; set; } = new ();
}
