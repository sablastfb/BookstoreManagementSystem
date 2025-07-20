using System.ComponentModel.DataAnnotations;

namespace BookstoreManagementSystem.WebApp.Domain;

public class Book
{
  public Guid Id { get; set; } 
  public decimal Price { get; set; }
  
  public string Title { get; init; } = string.Empty;
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
  
  public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
  public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
  public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
