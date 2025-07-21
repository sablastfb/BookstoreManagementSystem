using System.ComponentModel.DataAnnotations;

namespace BookstoreManagementSystem.WebApp.Domain;

public class Genre
{
  public short Id { get; init; } 
  
  [MinLength(1), MaxLength(50)]
  public required string Name { get; set; } 
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
}
