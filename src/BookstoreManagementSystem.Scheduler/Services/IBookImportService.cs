namespace BookstoreManagementSystem.Scheduler.Services;

public interface IBookImportService
{
  Task<int> ImportBooksAsync(); // Returns count of imported books
}
