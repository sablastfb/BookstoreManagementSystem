﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookstoreManagementSystem.WebApp.Domain;

public class Review
{
  [JsonIgnore]
  public long Id { get; init; } 
  [Range(1, 5)]
  public short Rating { get; set; }
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
  public Guid BookId { get; init; }
  public Book? Book { get; init; }
}
