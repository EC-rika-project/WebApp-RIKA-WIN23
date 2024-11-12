using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Interfaces;
using Infrastructure.DTOs;
using Infrastructure.Services;
using System.Linq.Expressions;
using WebApp.Controllers;
using WebApp.ViewModels;

public class AccountControllerTests
{

    private Mock<IUserService> _mock;
    private Mock<IPaymentService> _mockPayment;

    public AccountControllerTests()
    {
        _mock = new Mock<IUserService>();
        _mockPayment = new Mock<IPaymentService>();
    }

    // Tester nedan för "som en användare vill jag kunna ändra personliga uppgifter"

    //[Fact]
    //public void UpdateUserShouldReturn_OK()
    //{

    //    // Arrange


    //    UserModel userModel = new UserModel { Age = 21, Email = "hasse@gmail.com", Gender = Gender.Male, Name = "Hasse", ProfileImage = "www.kaksksa.se", UserId = "1" };

    //    ControllerResultModel result = new ControllerResultModel { Message = "Användaren uppdaterades", StatusCode = 200 };

    //    _mock.Setup(x => x.UpdateUserProfileAsync(userModel)).Returns(result);



    //    // Act

    //    ControllerResultModel finalResult = _mock.Object.UpdateUserProfileAsync(userModel);


    //    // Assert


    //    Assert.Equal(finalResult, result);
    //    Assert.Equal(finalResult.Message, result.Message);


    //}

    //[Fact]
    //public void AttemtToUpdateUserWithOneFieldEmptyShouldReturn_BadRequest()
    //{

    //    // Arrange


    //    UserModel userModel = new UserModel {Age = 0, Gender = Gender.Male, Name = "Hasse", ProfileImage = "www.kaksksa.se", UserId = "1" };

    //    ControllerResultModel result = new ControllerResultModel { Message = "Alla fält måste fyllas i.", StatusCode = 400 };

    //    _mock.Setup(x => x.UpdateUserProfileAsync(userModel)).Returns(result);



    //    // Act

    //    ControllerResultModel finalResult = _mock.Object.UpdateUserProfileAsync(userModel);


    //    // Assert


    //    Assert.Equal(finalResult, result);
    //    Assert.Equal(finalResult.Message, result.Message);


    //}


    [Fact]
    public void GetUserByIdShouldReturn_User()
    {
        // Arrange
        UserModel userModel = new UserModel { UserId = "2" };

		_mock.Setup(x => x.GetUserProfileByIdAsync(userModel.UserId)).Returns(userModel);

        // Act

        var result = _mock.Object.GetUserProfileByIdAsync(userModel.UserId);

        // Assert

        Assert.Equal(result, userModel);


	}

    [Fact]
    public void AttemptToGetUserByIdWithoutIdShouldReturn_EmptyUserModel()
    {
        // Arrange
        UserModel userModel = new UserModel { UserId = "" };

		_mock.Setup(x => x.GetUserProfileByIdAsync(userModel.UserId)).Returns(userModel);

        // Act
        var finalResult = _mock.Object.GetUserProfileByIdAsync(userModel.UserId);

        // Assert
        Assert.Equal(finalResult, userModel);
    }

	// Tester nedan för "lägga till och ändra betalningsmetoder"

    //Testa att spara betalningsuppgifter via formuläret.
    //Testa att uppdatera betalningsuppgifter via formuläret.
    //Testa att radera betalningsuppgifter via formuläret.
    //Testa att hämta betalningsuppgifter via formuläret.


	[Fact]
    public void UpdateUserPaymentMethodShouldReturn_OK()
    {
         //Arrange

        PaymentModel paymentModel = new PaymentModel {
            PaymentId = Guid.NewGuid().ToString(),
            UserId = "user-123",
            PaymentMethod = "Kreditkort", 
            CardHolderName = "Hasse Andersson",
            CardNumber = "1234-5678-9012-3456",
            ExpirationDate = "12/25",
            Cvc = "123", 
            PaymentStatus = "Pending", 
            Amount = 100.50m
        };

        ControllerResultModel result = new ControllerResultModel { Message = "Användarens betalningsinformation uppdaterades", StatusCode = 200 };

        _mockPayment.Setup(x => x.UpdatePaymentMethod(paymentModel)).Returns(result);

        //Act
        ControllerResultModel finalResult = _mockPayment.Object.UpdatePaymentMethod(paymentModel);

        //Assert
        Assert.Equal(finalResult, result);
        Assert.Equal(finalResult.Message, result.Message);

    }

    [Fact]
    public void AttemptToUpdateUserPaymentMethodWithFieldEmptyShouldReturn_Error()
    {

        //Arrange

        PaymentModel paymentModel = new PaymentModel
        {
            PaymentId = Guid.NewGuid().ToString(),
            UserId = "user-123",
            PaymentMethod = "Kreditkort",
            CardHolderName = "",
            CardNumber = "1234-5678-9012-3456",
            ExpirationDate = "12/25",
            Cvc = "123",
            PaymentStatus = "Pending",
            Amount = 100.50m
        };

        ControllerResultModel result = new ControllerResultModel { Message = "All information måste fyllas i", StatusCode = 400 };

        _mockPayment.Setup(x => x.UpdatePaymentMethod(paymentModel)).Returns(result);

        //Act
        ControllerResultModel finalResult = _mockPayment.Object.UpdatePaymentMethod(paymentModel);

        //Assert
        Assert.Equal(finalResult, result);
        Assert.Equal(finalResult.Message, result.Message);

    }

    [Fact]
    public void CreatePaymentMethodShouldReturn_Ok()
    {
        //Arrange

        PaymentModel paymentModel = new PaymentModel
        {
            PaymentId = Guid.NewGuid().ToString(),
            UserId = "user-123",
            PaymentMethod = "Kreditkort",
            CardHolderName = "Hasse Andersson",
            CardNumber = "1234-5678-9012-3456",
            ExpirationDate = "12/25",
            Cvc = "123",
            PaymentStatus = "Pending",
            Amount = 100.50m
        };

        ControllerResultModel result = new ControllerResultModel { Message = "Användarens betalningsinformation skapades", StatusCode = 200 };

        _mockPayment.Setup(x => x.CreatePaymentMethod(paymentModel)).Returns(result);

        //Act
        ControllerResultModel finalResult = _mockPayment.Object.CreatePaymentMethod(paymentModel);

        //Assert
        Assert.Equal(finalResult, result);
        Assert.Equal(finalResult.Message, result.Message);

    }

    [Fact]
    public async Task UpdateUserProfileInfoWithLocalApi_ShouldReturn_Ok()
    {

        // Arrange 
        UserDto userDto = new UserDto
        { UserId = "a27b6c49-93dc-40d1-a01d-edb3c7e2101d",
            FirstName = "Olle",
            LastName = "Olleson",
            Age = 5,
            Email = "Olle@Olle.olle",
            Gender = "Female",
            ProfileImageUrl = "/"
        };

        _mock.Setup(x => x.UpdateUserAsync(It.IsAny<UserDto>())).ReturnsAsync(true);


        //Act 
        var finalResult = await _mock.Object.UpdateUserAsync(userDto);

        //Assert 
        Assert.True(finalResult);

    }


    [Fact]
    public async Task UpdateUserProfile_ShouldReturn_False_WithEmptyFields()
    {
        //Arrange 

        UserDto userDto = new UserDto
        {
            UserId = "a27b6c49-93dc-40d1-a01d-edb3c7e2101d",
            FirstName = "Olle",
            LastName = "",
            Age = 2,
            Email = "Olle@Olle.olle",
            Gender = "",
            ProfileImageUrl = "/"
        };
        _mock.Setup(x => x.UpdateUserAsync(It.IsAny<UserDto>())).ReturnsAsync(false);


        //Act
        var finalResult = await _mock.Object.UpdateUserAsync(userDto);

        //Assert

        Assert.False(finalResult);
    }

    [Fact]
    public async Task UpdateUserProfile_ShouldReturn_False_WhenApiFails()
    {
        //Arrange 
        UserDto model = new UserDto
        {
            UserId = "a27b6c49-93dc-40d1-a01d-edb3c7e2101d",
            FirstName = "Olle",
            LastName = "",
            Age = 2,
            Email = "Olle@Olle.olle",
            Gender = "",
            ProfileImageUrl = "/"
        };
        _mock.Setup(x => x.UpdateUserAsync(It.IsAny<UserDto>())).ReturnsAsync(false);

        //Act
        var finalResult = await _mock.Object.UpdateUserAsync(model);

        //Assert

        Assert.False(finalResult);

    }


}



