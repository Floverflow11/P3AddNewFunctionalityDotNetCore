using System.Globalization;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.IntegrationTests;

public class ProductServiceTests : IClassFixture<CustomApplicationWebFactory>
{
    private readonly CustomApplicationWebFactory _factory;

    public ProductServiceTests(CustomApplicationWebFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public void ProductService_SaveProduct_ShouldAddProductToDatabase()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();

        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<P3Referential>();

        const string name = "TestProduct";
        const string description = "TestProductDescription";
        const string details = "TestProductDetails";
        const int stock = 75;
        const double price = 100;

        var newProductViewModel = new ProductViewModel
        {
            Name = name,
            Description = description,
            Details = details,
            Stock = stock.ToString(),
            Price = price.ToString(CultureInfo.InvariantCulture)
        };

        // Act
        productService.SaveProduct(newProductViewModel);

        // Assert
        var dbProduct = dbContext.Product.FirstOrDefault(p => p.Name == name);

        dbProduct.Should().NotBeNull();

        dbProduct!.Name.Should().Be(name);
        dbProduct.Description.Should().Be(description);
        dbProduct.Details.Should().Be(details);
        dbProduct.Quantity.Should().Be(stock);
        dbProduct.Price.Should().Be(price);
    }

    [Fact]
    public void ProductService_DeleteProduct_WhenProductIsInCart_ShouldDeleteFromCartAndDatabase()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();

        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<P3Referential>();
        var cart = scope.ServiceProvider.GetRequiredService<ICart>();

        var product = new Product
        {
            Name = "ToBeAddedAndDeletedProduct",
            Price = 10,
            Quantity = 5
        };

        dbContext.Product.Add(product);
        dbContext.SaveChanges();

        cart.AddItem(product, product.Quantity);
        
        cart.Lines.Should().HaveCount(1);
        cart.Lines.First().Product.Id.Should().Be(product.Id);
        
        dbContext.Product.Count().Should().Be(1);
        
        // Act
        productService.DeleteProduct(product.Id);
        
        // Assert
        var dbProduct = dbContext.Product.FirstOrDefault(p => p.Id == product.Id);
        dbProduct.Should().BeNull();
        
        cart.Lines.Should().BeEmpty();
    }
}