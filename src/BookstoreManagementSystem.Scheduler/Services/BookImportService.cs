
using BookstoreManagementSystem.WebApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.Scheduler.Services;

public class BookImportService(BookstoreDbContext dbContext): IBookImportService
{
  public async Task<int>  ImportBooksAsync()
  {
    return  await dbContext.Books.CountAsync();
  }
}
