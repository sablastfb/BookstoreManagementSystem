namespace BookstoreSystem.WebApp.Domain;

public class Book
{
  public Guid Id { get; }  = Guid.NewGuid();
  public decimal Price { get; init; }
  public string? Title { get; init; }
  
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
}
