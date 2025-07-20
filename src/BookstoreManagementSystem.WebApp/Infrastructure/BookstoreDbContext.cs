using BookstoreManagementSystem.WebApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Infrastructure;

public class BookstoreDbContext : DbContext
{
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
}


