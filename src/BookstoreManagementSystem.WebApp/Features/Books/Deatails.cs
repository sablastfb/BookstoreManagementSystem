using System.Net;
using BookstoreManagementSystem.WebApp.Infrastructure;
using BookstoreManagementSystem.WebApp.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookstoreManagementSystem.WebApp.Features.Books;

public class Deatails
{
  public record Query(Guid Id) : IRequest<BooksEnvelope>;
}
