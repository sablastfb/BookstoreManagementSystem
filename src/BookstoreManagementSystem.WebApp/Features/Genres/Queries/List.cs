﻿using BookstoreManagementSystem.WebApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Genres.Queries;

public class List
{
  public record Query() : IRequest<GenresEnvelope>;

  public class Handler(BookstoreDbContext context) : IRequestHandler<Query, GenresEnvelope>
  {
    public async Task<GenresEnvelope> Handle(Query request, CancellationToken cancellationToken)
    {
      var genres = await context.Genres
        .AsNoTracking()
        .OrderBy(a => a.Name)
        .ToListAsync(cancellationToken);

      var genresEnvelope = new GenresEnvelope(genres);
      genresEnvelope.Count = await context.Genres
        .AsNoTracking()
        .CountAsync(cancellationToken);
      return genresEnvelope;
    }
  }
}
