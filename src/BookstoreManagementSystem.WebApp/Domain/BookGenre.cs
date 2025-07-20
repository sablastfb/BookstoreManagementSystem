namespace BookstoreManagementSystem.WebApp.Domain;

public class BookGenre
{
  public Guid BookId { get; init; }
  public Book? Book { get; set; }
  public short GenreId { get; set; }
  public Genre? Genre { get; set; }
}
