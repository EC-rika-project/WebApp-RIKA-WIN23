using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using WebApp.Controllers;
using WebApp.ViewModels;
namespace Infrastructure_Tests
{
    public class SignUpController_Tests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly SignUpController _controller;

        public SignUpController_Tests()
        {
            // Skapar en mock för HttpMessageHandler för att kunna simulera HTTP-anrop
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                });

            var httpClient = new HttpClient(handlerMock.Object);

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _configurationMock = new Mock<IConfiguration>();
            var securityKeysSection = new Mock<IConfigurationSection>();
            securityKeysSection.Setup(x => x["WebAppKey"]).Returns("SomeSecurityKey");

                                                                            // Mockar sektionen SecurityKeys och ApiKeys och ställer in en nyckel
            var apiKeySection = new Mock<IConfigurationSection>();
            apiKeySection.Setup(x => x["Secret"]).Returns("SomeApiKey");

            _configurationMock.Setup(x => x.GetSection("SecurityKeys")).Returns(securityKeysSection.Object);
            _configurationMock.Setup(x => x.GetSection("ApiKey")).Returns(apiKeySection.Object);

            _controller = new SignUpController(_httpClientFactoryMock.Object, _configurationMock.Object);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        }

        [Fact]
        public async Task SignUp_WithValidData_ShouldRedirectToSignIn()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!"
            };

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SignIn", redirectResult.ActionName);
            Assert.Equal("Account created successfully. Please sign in.", _controller.TempData["Message"]);
        }

        [Fact]
        public async Task SignUp_WithExistingEmail_ShouldReturnViewWithErrorMessage()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Password = "Password123!"
            };

            // Skapa en ny mock av HttpMessageHandler som returnerar en HTTP Conflict
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Conflict,
                });

            var httpClient = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("An account with this email address already exists.", _controller.TempData["Message"]);
        }

        [Fact]
        public async Task SignUp_WithInvalidData_ShouldReturnViewWithModelErrors()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "",
                LastName = "",
                Email = "invalid-email",
                Password = ""
            };

            _controller.ModelState.AddModelError("Email", "Invalid email");

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(viewModel, viewResult.Model);
        }
    }

}

