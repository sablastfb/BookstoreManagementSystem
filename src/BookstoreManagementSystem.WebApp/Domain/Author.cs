using System.ComponentModel.DataAnnotations;

namespace BookstoreManagementSystem.WebApp.Domain;

public class Author
{
  public Guid Id { get; init; }
  
  [MinLength(1)]
  [MaxLength(200)]
  public required string Name { get; set; } 
  public int BirthYear { get; set; } 
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
}
