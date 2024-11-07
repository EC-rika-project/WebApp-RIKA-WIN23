using Xunit;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static WebApp.Controllers.ShoppingCartTests;
namespace WebApp.Controllers;
public class ShoppingCartTests
{
    // Test Case Description: Test that the user can see the correct total price for items in their shopping cart, and that the total price updates automatically when the quantity of items changes.


    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart
    {
        private List<Product> products = new();

        public void AddProduct(Product product)
        {
            products.Add(product);
        }

        public decimal GetTotalPrice()
        {
            return products.Sum(p => p.Price * p.Quantity);
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Quantity = quantity;
            }
        }
        public void RemoveProduct(int productId)
        {
            products.RemoveAll(p => p.Id == productId);
        }
    }

    public class CartController
    {
        public Cart Cart { get; set; }

        public CartController()
        {
            Cart = new Cart();
        }

        public decimal GetTotalPrice()
        {
            return Cart.GetTotalPrice();
        }
    }

    /// <summary>
    /// Test Case 1: Display total price for products in the shopping cart.
    /// Verifies that the total price is displayed correctly when products are added to the cart.
    /// Expects the total price to be the sum of product prices multiplied by their quantities.
    /// </summary>

    [Fact]
    public void AddProducts_ShouldUpdateTotalPrice_ThenReturnCorrectTotalPrice()
    {
        // Arrange
        var cart = new Cart();
        cart.AddProduct(new Product { Id = 1, Name = "Product 1", Price = 50, Quantity = 2 }); 
        cart.AddProduct(new Product { Id = 2, Name = "Product 2", Price = 30, Quantity = 1 }); 

        var controller = new CartController { Cart = cart };

        // Act
        var result = controller.GetTotalPrice();

        // Assert
        Assert.Equal(130, result); 
    }
    

    /// <summary>
    /// Test Case 2: Verify total price when the quantity of products increases.
    /// Tests that the total price is updated correctly when the quantity of a product in the cart increases.
    /// Expects the total price to adjust immediately to reflect the increased quantity.
    /// </summary>

    [Fact]
    public void IncreaseQuantityForOneProduct_ShouldUpdateTotalPrice_ThenReturnCorrectTotalPrice()
    {
        // Arrange
        var cart = new Cart();
        var product = new Product { Id = 1, Name = "Product 1", Price = 100, Quantity = 1 }; 
        cart.AddProduct(product);

        var controller = new CartController { Cart = cart };

        // Act
        product.Quantity = 2; 
        var updatedResult = controller.GetTotalPrice();

        // Assert
        Assert.Equal(200, updatedResult); 
    }

    /// <summary>
    /// Test Case 3: Verify total price when the quantity of products decreases.
    /// Tests that the total price is updated correctly when the quantity of a product in the cart decreases.
    /// Expects the total price to adjust immediately to reflect the decreased quantity.
    /// </summary>

    [Fact]
    public void DecreaseQuantityForOneProduct_ShouldUpdateTotalPrice_ThenReturnCorrectTotalPrice()
    {
        // Arrange
        var cart = new Cart();
        var product = new Product { Id = 1, Name = "Product 1", Price = 100, Quantity = 2 }; 
        cart.AddProduct(product);

        var controller = new CartController { Cart = cart };

        // Act
        product.Quantity = 1; 
        var updatedResult = controller.GetTotalPrice();

        // Assert
        Assert.Equal(100, updatedResult); 
    }

    /// <summary>
    /// Test Case 4: Verify total price when a product is removed.
    /// Tests that the total price is updated correctly when a product is removed from the cart.
    /// If the product is the last one in the cart, the total price is expected to be zero, and a message indicating an empty cart should be displayed.
    /// </summary>

    [Fact]
    public void RemoveProduct_ShouldUpdateTotalPrice_ThenReturnCorrectTotalPrice()
    {
        // Arrange
        var cart = new Cart();
        var product1 = new Product { Id = 1, Name = "Product 1", Price = 100, Quantity = 1 }; 
        var product2 = new Product { Id = 2, Name = "Product 2", Price = 50, Quantity = 1 };  
        cart.AddProduct(product1);
        cart.AddProduct(product2);

        var controller = new CartController { Cart = cart };

        // Act
        cart.RemoveProduct(product1.Id); 
        var updatedResultWithOneProduct = controller.GetTotalPrice();

        // Assert
        Assert.Equal(50, updatedResultWithOneProduct); 

        // Act
        cart.RemoveProduct(product2.Id);
        var updatedResultWithNoProduct = controller.GetTotalPrice();

        // Assert
        Assert.Equal(0, updatedResultWithNoProduct);
    }

    /// <summary>
    /// Test Case 5: Verify correct total price with multiple different products.
    /// Tests that the total price is calculated correctly when multiple different products with varying quantities are in the cart.
    /// Expects the total price to be the sum of each product's unit price multiplied by its quantity.
    /// </summary>
    [Fact]
    public void AddMultipleProducts_ShouldUpdateTotalPrice_ThenReturnCorrectTotalPrice()
        
    {
        // Arrange
        var cart = new Cart();
        var product1 = new Product { Id = 1, Name = "Product 1", Price = 100, Quantity = 2 }; 
        var product2 = new Product { Id = 2, Name = "Product 2", Price = 50, Quantity = 3 };  
        var product3 = new Product { Id = 3, Name = "Product 3", Price = 25, Quantity = 4 };  
        cart.AddProduct(product1);
        cart.AddProduct(product2);
        cart.AddProduct(product3);

        var controller = new CartController { Cart = cart };

        // Act
        var result = controller.GetTotalPrice();

        // Assert
        Assert.Equal(450, result); 
    }

    /// <summary>
    /// Test Case 6: Update total price when changing the quantity of a product with multiple products in the cart.
    /// Tests that the total price is updated correctly when the quantity of one of the products is changed.
    /// </summary>
    [Fact]
    public void ChangeQuantityForOneProduct_ShouldUpdateTotalPrice_ThenReturnCorrectTotalPrice()
         
    {
        // Arrange
        var cart = new Cart();
        var product1 = new Product { Id = 1, Name = "Product 1", Price = 100, Quantity = 2 }; 
        var product2 = new Product { Id = 2, Name = "Product 2", Price = 50, Quantity = 3 };  
        cart.AddProduct(product1);
        cart.AddProduct(product2);

        var controller = new CartController { Cart = cart };

        // Act 
        product2.Quantity = 4;
        var updatedResult = controller.GetTotalPrice();

        // Assert
        Assert.Equal(400, updatedResult);
    }

    /// <summary>
    /// Test Case 7: Update total price when modifying cart contents.
    /// Tests that the total price is updated correctly when quantities of multiple products are changed and a product is removed from the cart.
    /// Expects the total price to reflect the latest cumulative cost based on the most recent changes.
    /// </summary>
    [Fact]
    public void ChangeQuantityOrRemoveProducts_ShouldUpdateTotalPrice_ThenReturnCorrectTotalPrice()
    {
        // Arrange
        var cart = new Cart();
        var product1 = new Product { Id = 1, Name = "Product 1", Price = 100, Quantity = 2 }; 
        var product2 = new Product { Id = 2, Name = "Product 2", Price = 50, Quantity = 3 };  
        var product3 = new Product { Id = 3, Name = "Product 3", Price = 25, Quantity = 4 };  
        cart.AddProduct(product1);
        cart.AddProduct(product2);
        cart.AddProduct(product3);

        var controller = new CartController { Cart = cart };

        // Act
        var initialTotalPrice = controller.GetTotalPrice();
        Assert.Equal(450, initialTotalPrice);

        // Act
        product1.Quantity = 3; 
        var totalPriceAfterIncrease = controller.GetTotalPrice();
        Assert.Equal(550, totalPriceAfterIncrease); 

        // Act
        product2.Quantity = 1; 
        var totalPriceAfterDecrease = controller.GetTotalPrice();
        Assert.Equal(450, totalPriceAfterDecrease); 

        // Act
        cart.RemoveProduct(product3.Id);
        var totalPriceAfterRemoval = controller.GetTotalPrice();

        // Assert
        Assert.Equal(350, totalPriceAfterRemoval); 
    }
}