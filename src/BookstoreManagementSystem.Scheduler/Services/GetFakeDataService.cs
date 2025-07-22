using System.Net.Http.Headers;

namespace BookstoreManagementSystem.Scheduler.Services;

public class GetFakeDataService() : IGetFakeDataService
{
  public HttpClient? _client = null;

  private HttpClient CreateClient()
  {
    _client = new HttpClient();
    _client.BaseAddress = new Uri("https://localhost:5000");
    _client.DefaultRequestHeaders.Accept.Add(
      new MediaTypeWithQualityHeaderValue("text/plain"));
    return _client;
  }
  
  public void GetFakeBooks(int count, int typoCount)
  {
    _client ??= CreateClient();
    var response =  _client.GetAsync($"/getBooks/{count}/{typoCount}");
  }
}
