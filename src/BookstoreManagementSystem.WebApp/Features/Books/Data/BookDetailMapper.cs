using BookstoreManagementSystem.WebApp.Features.Books.Queries;

namespace BookstoreManagementSystem.WebApp.Features.Books.Data;

public static class BookDetailMapper
{
  public static BooksDetailsEnvelope MapToEnvelope(IEnumerable<DetailsList.BookDetailDto> dtos)
  {
    var envelopeItems = dtos.Select(dto => new BookDetailEnvelope(
      Title: dto.Title,
      Price: dto.Price,
      AverageRating: dto.AverageRating,
      AuthorNames: SplitStringToList(dto.AuthorNames),
      GenreNames: SplitStringToList(dto.GenreNames)
    )).ToList();

    return new BooksDetailsEnvelope(envelopeItems);
  }
  
  private static List<string> SplitStringToList(string input)
  {
    if (string.IsNullOrWhiteSpace(input))
      return new List<string>();

    return input.Split('|')
      .Select(x => x.Trim())
      .Where(x => !string.IsNullOrWhiteSpace(x))
      .ToList();
  }
}
