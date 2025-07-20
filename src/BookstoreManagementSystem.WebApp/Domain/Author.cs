namespace BookstoreManagementSystem.WebApp.Domain;

public class Author
{
  public Guid Id { get; init; }
  public string Name { get; init; } = string.Empty; 
  public int BirthYear { get; init; } 
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
}
