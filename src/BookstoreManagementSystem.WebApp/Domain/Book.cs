using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookstoreManagementSystem.WebApp.Domain;

public class Book
{
  public Guid Id { get; set; } 
  public decimal Price { get; set; }
  
  [MinLength(1), MaxLength(200)]
  public required string Title { get; init; }
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
  
  public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
  public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
  public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
