namespace BookstoreManagementSystem.WebApp.Domain;

public class Author
{
  public Guid Id { get; }  = Guid.NewGuid();
  public string? FirstName { get; init; }
  public string? LastName { get; init;}
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
}
