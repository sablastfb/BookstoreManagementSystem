using BookstoreManagementSystem.WebApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookstoreManagementSystem.WebApp.Infrastructure;

public class BookstoreDbContext : DbContext
{
  private IDbContextTransaction? _currentTransaction;
  public DbSet<Book> Books { get; set; }
  public DbSet<Author> Authors { get; set; }
  public DbSet<Genre> Genres { get; set; }
  public DbSet<Review> Reviews { get; set; }
  public DbSet<BookAuthor> BookAuthors { get; set; }
  public DbSet<BookGenre> BookGenres { get; set; }
  
  public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<BookAuthor>().HasKey(ba => new { ba.BookId, ba.AuthorId });
    modelBuilder.Entity<BookGenre>().HasKey(bg => new { bg.BookId, bg.GenreId });
  }
  
  public void BeginTransaction()
  {
    if (_currentTransaction != null)
    {
      return;
    }
  }

  public void CommitTransaction()
  {
    try
    {
      _currentTransaction?.Commit();
    }
    catch
    {
      RollbackTransaction();
      throw;
    }
    finally
    {
      if (_currentTransaction != null)
      {
        _currentTransaction.Dispose();
        _currentTransaction = null;
      }
    }
  }

  public void RollbackTransaction()
  {
    try
    {
      _currentTransaction?.Rollback();
    }
    finally
    {
      if (_currentTransaction != null)
      {
        _currentTransaction.Dispose();
        _currentTransaction = null;
      }
    }
  }
}


