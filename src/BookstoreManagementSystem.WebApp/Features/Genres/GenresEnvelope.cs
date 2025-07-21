using BookstoreManagementSystem.WebApp.Domain;

namespace BookstoreManagementSystem.WebApp.Features.Genres;

public record GenresEnvelope(List<Genre> Genres)
{
  public int Count { get; set; }
}
