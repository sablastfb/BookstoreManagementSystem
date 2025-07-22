using System.ComponentModel.DataAnnotations;

namespace BookstoreDataGenerator.Models;

public class Author
{
  [MinLength(1), MaxLength(200)]
  public required string Name { get; set; }
  public int BirthYear { get; set; }
}
