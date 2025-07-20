namespace BookstoreManagementSystem.WebApp.Domain;

public class BookAuthor
{
  public Guid BookId { get; init; }
  public Book? Book { get; init;}
  public Guid AuthorId { get; init; }
  public Author? Author { get; init; }
}
