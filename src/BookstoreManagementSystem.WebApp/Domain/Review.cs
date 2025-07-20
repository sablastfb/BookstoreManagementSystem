﻿namespace BookstoreManagementSystem.WebApp.Domain;

public class Review
{
  public long Id { get; init; } 
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; set; }
}
