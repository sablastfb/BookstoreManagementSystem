using System.ComponentModel.DataAnnotations;

namespace BookstoreManagementSystem.WebApp.Domain;

public class Author
{
  public Guid Id { get; init; }
  
  [MinLength(1)]
  [MaxLength(200)]
  public string? Name { get; init; } 
  public int BirthYear { get; init; } 
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
}
