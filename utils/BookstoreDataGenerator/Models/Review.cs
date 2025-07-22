namespace BookstoreDataGenerator.Models;

public class Review
{
  public short Rating { get; set; }
  public Guid BookId { get; init; }
}
