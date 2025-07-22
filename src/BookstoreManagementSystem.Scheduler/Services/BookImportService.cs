
using BookstoreManagementSystem.WebApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.Scheduler.Services;

public class BookImportService(BookstoreDbContext dbContext, IGetFakeDataService getFakeData): IBookImportService
{
  public async Task<int>  ImportBooksAsync()
  {
    var books = await getFakeData.GetFakeBooks(1, 100);
    
    return  await dbContext.Books.CountAsync();
  }
}
