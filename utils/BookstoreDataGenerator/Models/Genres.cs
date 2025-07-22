using System.ComponentModel.DataAnnotations;

namespace BookstoreDataGenerator.Models;

public class Genre
{
  [MinLength(1), MaxLength(50)]
  public required string Name { get; set; }
}
