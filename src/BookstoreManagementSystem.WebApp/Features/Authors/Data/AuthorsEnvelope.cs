using BookstoreManagementSystem.WebApp.Domain;

namespace BookstoreManagementSystem.WebApp.Features.Authors;

public record AuthorsEnvelope(List<Author> Authors)
{
  public int Count { get; set; }
};
