using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookstoreDataGenerator.Utils;

public static class TestLogIn
{
  private static HttpClient? _client;

  public static async Task<HttpClient> LogInClient(IConfiguration configuration)
  {
    if (_client != null) return _client;
    _client = new HttpClient();
    _client.BaseAddress = new Uri("http://localhost:5013");

    _client.DefaultRequestHeaders.Accept.Add(
      new MediaTypeWithQualityHeaderValue("text/plain"));

    var loginData = new
    {
      user = new { userName = configuration["Auth:Username"], password = configuration["Auth:Password"] }
    };

    var jsonContent = new StringContent(
      JsonSerializer.Serialize(loginData,
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
      Encoding.UTF8,
      "application/json");

    var response = await _client.PostAsync("/api/v1/Users/login", jsonContent);

    response.EnsureSuccessStatusCode();

    var responseContent = await response.Content.ReadAsStringAsync();
    var responseObject = JsonSerializer.Deserialize<LoginResponse>(responseContent);

    if (!string.IsNullOrEmpty(responseObject?.Token))
    {
      _client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", responseObject.Token);
    }

    return _client;
  }

  public class LoginResponse
  {
    [JsonPropertyName("token")] 
    public string Token { get; set; } = string.Empty;
  }
}
