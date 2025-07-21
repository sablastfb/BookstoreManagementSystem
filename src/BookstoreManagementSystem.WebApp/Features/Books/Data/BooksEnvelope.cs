using BookstoreManagementSystem.WebApp.Domain;

namespace BookstoreManagementSystem.WebApp.Features.Books.Data;

public record BooksEnvelope(List<Book> Books)
{
  public int Count {get; set;}
}
