using System.Linq;
using FluentAssertions;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;
using ResourcesProductService = P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService;

namespace P3AddNewFunctionalityDotNetCore.UnitTests;

public class ProductViewModelTests
{
    [Fact]
    public void ProductViewModel_WithAllValidProperties_ShouldPassValidation()
    {
        // Arrange
        var viewModel = new ProductViewModel
        {
            Id = 1,
            Name = "Product",
            Description = "Description",
            Details = "Details",
            Stock = "1",
            Price = "0.10"
        };

        // Act
        var results = ProductViewModelUtils.ValidateModel(viewModel);

        // Assert
        results.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ProductViewModel_WithEmptyOrNullName_ReturnsMissingName(string missingName)
    {
        // Arrange
        var viewModel = new ProductViewModel
        {
            Id = 1,
            Name = missingName,
            Description = "Description",
            Details = "Details",
            Stock = "1",
            Price = "0.10"
        };

        var expectedErrorMessage = ResourcesProductService.MissingName;

        // Act
        var results = ProductViewModelUtils.ValidateModel(viewModel);

        // Assert
        var error = results.Should().ContainSingle().Subject;

        error.MemberNames.Should().Contain(nameof(ProductViewModel.Name));
        error.ErrorMessage.Should().Be(expectedErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ProductViewModel_WithEmptyOrNullPrice_ReturnsMissingPrice(string missingPrice)
    {
        // Arrange
        var viewModel = new ProductViewModel
        {
            Id = 1,
            Name = "Product",
            Description = "Description",
            Details = "Details",
            Stock = "1",
            Price = missingPrice
        };

        var expectedErrorMessage = ResourcesProductService.MissingPrice;

        // Act
        var results = ProductViewModelUtils.ValidateModel(viewModel);

        // Assert
        var error = results.Should().ContainSingle().Subject;

        error.MemberNames.Should().Contain(nameof(ProductViewModel.Price));
        error.ErrorMessage.Should().Be(expectedErrorMessage);
    }

    [Fact]
    public void ProductViewModel_WithLettersAsPrice_ReturnsPriceNotANumber()
    {
        const string letters = "abc";

        // Arrange
        var viewModel = new ProductViewModel
        {
            Id = 1,
            Name = "Product",
            Description = "Description",
            Details = "Details",
            Stock = "1",
            Price = letters
        };

        var expectedErrorMessage = ResourcesProductService.PriceNotANumber;

        // Act
        var results = ProductViewModelUtils.ValidateModel(viewModel);

        // Assert
        results.Should().Contain(error =>
            error.MemberNames.Contains(nameof(ProductViewModel.Price)) &&
            error.ErrorMessage == expectedErrorMessage
        );
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-0.99")]
    public void ProductViewModel_WithNegativeOrZeroPrice_ReturnsPriceNotGreaterThanZero(string negativeOrZeroPrice)
    {
        // Arrange
        var viewModel = new ProductViewModel
        {
            Id = 1,
            Name = "Product",
            Description = "Description",
            Details = "Details",
            Stock = "1",
            Price = negativeOrZeroPrice
        };

        var expectedErrorMessage = ResourcesProductService.PriceNotGreaterThanZero;

        // Act
        var results = ProductViewModelUtils.ValidateModel(viewModel);

        // Assert
        var error = results.Should().ContainSingle().Subject;

        error.MemberNames.Should().Contain(nameof(ProductViewModel.Price));
        error.ErrorMessage.Should().Be(expectedErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ProductViewModel_WithEmptyOrNullStock_ReturnsMissingStock(string missingStock)
    {
        // Arrange
        var viewModel = new ProductViewModel
        {
            Id = 1,
            Name = "Product",
            Description = "Description",
            Details = "Details",
            Stock = missingStock,
            Price = "0.10"
        };

        var expectedErrorMessage = ResourcesProductService.MissingStock;

        // Act
        var results = ProductViewModelUtils.ValidateModel(viewModel);

        // Assert
        var error = results.Should().ContainSingle().Subject;

        error.MemberNames.Should().Contain(nameof(ProductViewModel.Stock));
        error.ErrorMessage.Should().Be(expectedErrorMessage);
    }

    [Fact]
    public void ProductViewModel_WithNonIntegerStock_ReturnsStockNotAnInteger()
    {
        // Arrange
        var viewModel = new ProductViewModel
        {
            Id = 1,
            Name = "Product",
            Description = "Description",
            Details = "Details",
            Stock = "1.5",
            Price = "0.10"
        };

        var expectedErrorMessage = ResourcesProductService.StockNotAnInteger;

        // Act
        var results = ProductViewModelUtils.ValidateModel(viewModel);

        // Assert
        results.Should().Contain(error =>
            error.MemberNames.Contains(nameof(ProductViewModel.Stock)) &&
            error.ErrorMessage == expectedErrorMessage
        );
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    public void ProductViewModel_WithNegativeOrZeroStock_ReturnsStockNotGreaterThanZero(string negativeOrZeroStock)
    {
        // Arrange
        var viewModel = new ProductViewModel
        {
            Id = 1,
            Name = "Product",
            Description = "Description",
            Details = "Details",
            Stock = negativeOrZeroStock,
            Price = "0.10"
        };

        var expectedErrorMessage = ResourcesProductService.StockNotGreaterThanZero;

        // Act
        var results = ProductViewModelUtils.ValidateModel(viewModel);

        // Assert
        var error = results.Should().ContainSingle().Subject;

        error.MemberNames.Should().Contain(nameof(ProductViewModel.Stock));
        error.ErrorMessage.Should().Be(expectedErrorMessage);
    }
}