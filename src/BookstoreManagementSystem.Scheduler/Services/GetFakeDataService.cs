using System.Net.Http.Headers;
using System.Text.Json;
using BookstoreManagementSystem.Scheduler.Models;

namespace BookstoreManagementSystem.Scheduler.Services;

public class GetFakeDataService() : IGetFakeDataService
{
  public HttpClient? _client = null;

  private HttpClient CreateClient()
  {
    _client = new HttpClient();
    _client.BaseAddress = new Uri("http://localhost:5000");
    _client.DefaultRequestHeaders.Accept.Add(
      new MediaTypeWithQualityHeaderValue("text/plain"));
    return _client;
  }
  
  public async Task<List<Book>> GetFakeBooks(int count, int typoCount = 0)
  {
    _client ??= CreateClient();
    var response = await _client.GetAsync($"/getBooks/{count}/{typoCount}");
    response.EnsureSuccessStatusCode();
    var booksContent = await response.Content.ReadAsStringAsync();
    var book = JsonSerializer.Deserialize<List<Book>>(booksContent);
    return book!;
  }
}
