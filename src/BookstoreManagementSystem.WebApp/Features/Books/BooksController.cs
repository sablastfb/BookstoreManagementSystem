using Microsoft.AspNetCore.Mvc;

namespace BookstoreManagementSystem.WebApp.Features.Books;

[Route("books")]
public class BooksController : Controller
{
  [HttpGet]
  public  Task Help()
  {
    return Task.CompletedTask;
  }
}
