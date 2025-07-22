using BookstoreManagementSystem.WebApp.Domain;
using BookstoreManagementSystem.WebApp.Features.Books.Commands;
using BookstoreManagementSystem.WebApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BookstoreManagementSystem.IntegrationTests.Features.Books
{
    public class CreateBookCommandIntegrationTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgreSqlContainer;
        private BookstoreDbContext _dbContext = null!;
        private Create.Handler _handler = null!;

        public CreateBookCommandIntegrationTests()
        {
            _postgreSqlContainer = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("bookstore_test")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();

            var options = new DbContextOptionsBuilder<BookstoreDbContext>()
                .UseNpgsql(_postgreSqlContainer.GetConnectionString())
                .Options;

            _dbContext = new BookstoreDbContext(options);
            await _dbContext.Database.EnsureCreatedAsync();
            await SeedTestDataAsync();
            _handler = new Create.Handler(_dbContext);
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
            await _postgreSqlContainer.DisposeAsync();
        }

        private async Task SeedTestDataAsync()
        {
            // Seed authors
            var authors = new List<Author>
            {
                new() { Id = Guid.NewGuid(), Name = "Author 1" },
                new() { Id = Guid.NewGuid(), Name = "Author 2" }
            };

            // Seed genres
            var genres = new List<Genre>
            {
                new() { Id = 1, Name = "Fiction" },
                new() { Id = 2, Name = "Science Fiction" },
                new() { Id = 3, Name = "Fantasy" }
            };

            await _dbContext.Authors.AddRangeAsync(authors);
            await _dbContext.Genres.AddRangeAsync(genres);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateBook_WithValidData_ShouldPersistInDatabase()
        {
            // Arrange
            var authors = await _dbContext.Authors.ToListAsync();
            var authorIds = authors.Select(a => a.Id).ToArray();
            
            var command = new Create.Command(new Create.BookData
            {
                Price = 29.99m,
                Title = "Integration Test Book",
                AuthorList = authorIds,
                GenreList = new short[] { 1, 2 } // Fiction and Science Fiction
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert - Check the book was created
            var bookInDb = await _dbContext.Books
                .FirstOrDefaultAsync(b => b.Id == result.Book.Id);
            
            Assert.NotNull(bookInDb);
            Assert.Equal("Integration Test Book", bookInDb.Title);
            Assert.Equal(29.99m, bookInDb.Price);

            // Check author relationships
            var bookAuthors = await _dbContext.BookAuthors
                .Include(ba => ba.Author)
                .Where(ba => ba.BookId == result.Book.Id)
                .ToListAsync();
            
            Assert.Equal(authorIds.Length, bookAuthors.Count);
            Assert.All(bookAuthors, ba => Assert.Contains(ba.AuthorId, authorIds));

            // Check genre relationships
            var bookGenres = await _dbContext.BookGenres
                .Include(bg => bg.Genre)
                .Where(bg => bg.BookId == result.Book.Id)
                .ToListAsync();
            
            Assert.Equal(2, bookGenres.Count);
            Assert.Contains(bookGenres, bg => bg.GenreId == 1);
            Assert.Contains(bookGenres, bg => bg.GenreId == 2);
        }

        [Fact]
        public async Task CreateBook_WithNonExistingAuthor_ShouldThrowException()
        {
            // Arrange
            var nonExistingAuthorId = Guid.NewGuid();
            var command = new Create.Command(new Create.BookData
            {
                Price = 29.99m,
                Title = "Book with invalid author",
                AuthorList = new[] { nonExistingAuthorId },
                GenreList = new short[] { 1 }
            });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            Assert.Equal($"Author with ID={nonExistingAuthorId} not found", exception.Message);
        }
    }
}
