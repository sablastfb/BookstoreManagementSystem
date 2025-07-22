using BookstoreManagementSystem.Scheduler.Models;

namespace BookstoreManagementSystem.Scheduler.Services;

public interface IGetFakeDataService
{
  Task<List<Book>> GetFakeBooks(int count, int typoCount = 0);
}
