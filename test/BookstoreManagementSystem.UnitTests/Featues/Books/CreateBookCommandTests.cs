using BookstoreManagementSystem.WebApp.Features.Books.Commands;
using FluentValidation.TestHelper;

namespace BookstoreManagementSystem.UnitTests.Featues.Books;

public class CreateBookCommandTests
{
  private readonly Create.CommandValidator _validator;

  public CreateBookCommandTests()
  {
    _validator = new Create.CommandValidator();
  }
  
  [Fact]
  public void Validator_WhenAllFieldsAreValid_ShouldNotHaveValidationErrors()
  {
    // Arrange
    var command = new Create.Command(new Create.BookData
    {
      Price = 19.99m,
      Title = "Test Book",
      AuthorList = new[] { Guid.NewGuid() },
      GenreList = new short[] { 1 }
    });

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }
  
  
  [Fact]
  public void Validator_WhenPriceIsZero_ShouldHaveValidationError()
  {
    // Arrange
    var command = new Create.Command(new Create.BookData
    {
      Price = 0,
      Title = "Test Book",
      AuthorList = new[] { Guid.NewGuid() },
      GenreList = new short[] { 1 }
    });

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.BookData.Price)
      .WithErrorMessage("Price must be positive");
  }
  
}
