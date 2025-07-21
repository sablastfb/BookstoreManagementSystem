namespace BookstoreManagementSystem.WebApp.Features.Books.Data;

public record BookDetailEnvelope(
  string Title,
  decimal Price,
  double AverageRating,
  List<string> AuthorNames,
  List<string> GenreNames);
