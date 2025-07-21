using MediatR;

namespace BookstoreManagementSystem.WebApp.Infrastructure;

public class DBContextTransactionPipelineBehavior<TRequest, TResponse>(BookstoreDbContext context)
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : notnull
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken
  )
  {
    TResponse? result;

    try
    {
      context.BeginTransaction();

      result = await next();

      context.CommitTransaction();
    }
    catch (Exception)
    {
      context.RollbackTransaction();
      throw;
    }

    return result;
  }
}
