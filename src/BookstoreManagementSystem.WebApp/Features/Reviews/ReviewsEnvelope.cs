using BookstoreManagementSystem.WebApp.Domain;

namespace BookstoreManagementSystem.WebApp.Features.Reviews;

public record ReviewsEnvelope(List<Review> Reviews)
{
  public int Count { get; set; }
}
