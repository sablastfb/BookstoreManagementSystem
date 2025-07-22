using BookDomain = BookstoreManagementSystem.WebApp.Domain.Book;
using BookstoreManagementSystem.WebApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FuzzySharp;
using Microsoft.Extensions.Logging;
using Book = BookstoreManagementSystem.Scheduler.Models.Book;

namespace BookstoreManagementSystem.Scheduler.Services;

public class BookImportService(BookstoreDbContext dbContext, IGetFakeDataService getFakeData,  ILogger<BookImportService> logger): IBookImportService
{
  private const int BatchSize = 5000;
  private const int SimilarityThreshold = 95; 
  public async Task<int>  ImportBooksAsync()
  {
    var booksToImport = await getFakeData.GetFakeBooks(500, 500); 
    
    var existingTitlesList = await dbContext.Books
      .AsNoTracking()
      .Select(b => b.Title.Trim().ToLower()).ToListAsync();
    var existingTitlesSet = new  HashSet<string>(existingTitlesList);
    
    var newBooks = new List<Book>();
    
    foreach (var book in booksToImport)
    {
      var normalizedTitle = book.Title.Trim().ToLower();
      
      if (existingTitlesSet.Contains(normalizedTitle))
      {
        logger.LogInformation("Removed duplicate book by strick criteria");
        continue;
      }
      var bestMatch = Process.ExtractOne(
        normalizedTitle,
        existingTitlesList);

      if (bestMatch != null && bestMatch.Score >= SimilarityThreshold)
      {
        logger.LogInformation("Removed duplicate book by fuzzy criteria. In DB '{}'. From api '{}' ",bestMatch.Value, normalizedTitle);
        continue;
      }
      newBooks.Add(book);
      existingTitlesList.Add(normalizedTitle);
      existingTitlesSet.Add(normalizedTitle);
    }
    List<BookDomain> books = newBooks.Select(b => new BookDomain
    {
      Title = b.Title,
      Price = b.Price,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow,
    }).ToList();
    await dbContext.BulkInsertAsync(books);
    dbContext.SaveChanges();
    return newBooks.Count();
  }
}
