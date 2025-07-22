using System.Text.Json;
using BookstoreDataGenerator.Models;

namespace BookstoreDataGenerator.Utils;

public static class FakerUtil
{
  public static async Task<List<Book>> GenerateBooks(int count, HttpClient logInClient)
  {
    var authorsResponse = await logInClient.GetAsync("/api/v1/Authors");
      authorsResponse.EnsureSuccessStatusCode();
      var authorContent = await authorsResponse.Content.ReadAsStringAsync();
      var authors = JsonSerializer.Deserialize<AuthorsResponse>(authorContent);
      
      var genreResponse = await logInClient.GetAsync("/api/v1/Genres");
      authorsResponse.EnsureSuccessStatusCode();
      var genreContent = await genreResponse.Content.ReadAsStringAsync();
      var genres = JsonSerializer.Deserialize<GenreResponse>(genreContent);
      
      return BookstoreDataFaker.GenerateFakeBook(count, authors!.Authors, genres!.Genres);
  }
}
