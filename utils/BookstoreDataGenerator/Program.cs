using System.Text.Json;
using System.Text.Json.Serialization;
using BookstoreDataGenerator.Models;
using BookstoreDataGenerator.Utils;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/addAuthors", async (AddRequest request, IConfiguration configuration) =>
{
  var logInClient = await LogInClientUntil.LogInClient(configuration);
  var authors = BookstoreDataFaker.GenerateFakeAuthors(request.Count);

  foreach (var author in authors)
  {
    var authorData = new { author = new { name = author.Name, birthYear = author.BirthYear } };
    var jsonContent = DataUtil.JsonContentHelper(authorData);
    await logInClient.PostAsync("/api/v1/Authors", jsonContent);
  }

  return Results.Ok();
});

app.MapPost("/addGenres", async (IConfiguration configuration) =>
{
  var logInClient = await LogInClientUntil.LogInClient(configuration);
  var genres = BookstoreDataFaker.GenerateFakeGenres();

  foreach (var genre in genres)
  {
    var genresData = new { genre = new { name = genre.Name } };
    var jsonContent = DataUtil.JsonContentHelper(genresData);
    await logInClient.PostAsync("/api/v1/Genres", jsonContent);
  }

  return Results.Ok();
});

app.MapPost("/addBooks", async (AddRequest request, IConfiguration configuration) =>
{
  var logInClient = await LogInClientUntil.LogInClient(configuration);
  var books = await FakerUtil.GenerateBooks(request.Count, logInClient);

  foreach (var book in books)
  {
    var bookData = new { bookData = new
    {
      price = book.BookData.Price, title = book.BookData.Title,
      authorList = book.BookData.AuthorList,
      genreList = book.BookData.GenreList
    } };
    var jsonContent = DataUtil.JsonContentHelper(bookData);
    await logInClient.PostAsync("/api/v1/Books", jsonContent);
  }
  return Results.Ok();
});

app.MapPost("/addReviews", async (AddRequest request, IConfiguration configuration) =>
{
  var logInClient = await LogInClientUntil.LogInClient(configuration);

  var authorsResponse = await logInClient.GetAsync("/api/v1/Books");
  authorsResponse.EnsureSuccessStatusCode();
  var authorContent = await authorsResponse.Content.ReadAsStringAsync();
  var book = JsonSerializer.Deserialize<BooksResponse>(authorContent);
  
  var reviews = BookstoreDataFaker.GenerateFakeReviews(request.Count, book!.Books);
  foreach (var review in reviews)
  {
    var bookData = new { review = new
    {
      bookId = review.BookId,
      rating = review.Rating,
    } };
    var jsonContent = DataUtil.JsonContentHelper(bookData);
    await logInClient.PostAsync("/api/v1/Reviews", jsonContent);
  }

  return Results.Ok();
});

app.MapGet("/getBooks/{count}/{typoCount}", async (int count, int typoCount,IConfiguration configuration) =>
{
  var logInClient = await LogInClientUntil.LogInClient(configuration);
  var books = await FakerUtil.GenerateBooks(count, logInClient);
  
  // Simulate typos
  for (int i = 0; i < typoCount; i++)
  {
    Random random = new Random();
    int randomIndex = random.Next(books.Count);
    var randomBook = books[randomIndex];
    var book = new Book()
    {
      BookData = new BookData()
      {
        AuthorList = randomBook.BookData.AuthorList,
        GenreList = randomBook.BookData.GenreList,
        Price = randomBook.BookData.Price,
        Title = TypoGenerator.AddTypo(randomBook.BookData.Title),
      }
    };
    books.Add(book);
  }
  
  return Results.Ok(books);
});


app.Run();
