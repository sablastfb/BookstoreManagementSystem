using BookstoreManagementSystem.Scheduler.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace BookstoreManagementSystem.Scheduler.Jobs;

public class BookImportJob(IBookImportService importService, ILogger<BookImportJob> logger ) : IJob
{
  public async Task Execute(IJobExecutionContext context)
  { 
    logger.LogInformation("Starting book import...");
    var numberOfImportedBooks = await importService.ImportBooksAsync();
    logger.LogInformation("Book import completed. Imported {}",numberOfImportedBooks);
  }
}
