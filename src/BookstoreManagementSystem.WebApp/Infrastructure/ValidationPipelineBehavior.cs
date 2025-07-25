﻿using FluentValidation;
using MediatR;

namespace BookstoreManagementSystem.WebApp.Infrastructure;

public class ValidationPipelineBehavior<TRequest, TResponse>(
  IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : notnull
{
  private readonly List<IValidator<TRequest>> _validators = validators.ToList();

  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken
  )
  {
    var context = new ValidationContext<TRequest>(request);
    var failures = _validators
      .Select(v => v.Validate(context))
      .SelectMany(result => result.Errors)
      .Where(f => f != null)
      .ToList();

    if (failures.Count != 0)
    {
      throw new ValidationException(failures);
    }

    return await next();
  }
}
