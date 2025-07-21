using BookstoreManagementSystem.WebApp.Domain;

namespace BookstoreManagementSystem.WebApp.Features.Reviews.Data;

public record ReviewsEnvelope(List<Review> Reviews)
{
  public int Count { get; set; }
}
