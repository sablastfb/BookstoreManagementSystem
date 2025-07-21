using BookstoreManagementSystem.WebApp.Domain;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public record BooksEnvelope(List<Book> Books)
{
  public int Count {get; set;}
}
