namespace BookstoreManagementSystem.WebApp.Features.Books.Data;

public record BooksDetailsEnvelope(List<BookDetailEnvelope> BooksDetails)
{
  public int Count {get; set;}
}
