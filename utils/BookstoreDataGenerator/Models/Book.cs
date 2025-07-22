namespace BookstoreDataGenerator.Models;

public class BookData
{
  public decimal Price { get; set; }
  public string Title { get; set; } = string.Empty;
  public List<Guid> AuthorList { get; set; } = new List<Guid>();
  public List<short> GenreList { get; set; } = new List<short>();
}

public class Book
{
  public BookData BookData { get; set; } = new BookData();
}
