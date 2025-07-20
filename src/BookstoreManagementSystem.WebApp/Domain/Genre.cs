namespace BookstoreManagementSystem.WebApp.Domain;

public class Genre
{
  public short Id { get; init; } 
  public string Name { get; init; } = string.Empty; 
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
}
