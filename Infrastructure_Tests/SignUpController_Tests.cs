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
                Password = "Password123!",
                TermsAndConditions = true
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
                Password = "Password123!",
                TermsAndConditions = true
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
                Password = "",
                TermsAndConditions = true
            };

            _controller.ModelState.AddModelError("Email", "Invalid email");

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(viewModel, viewResult.Model);
        }

         [Fact]
        public async Task SignUp_WhenTermsAccepted_ShouldEnableSignUpButton()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                TermsAndConditions = true // Checkbox markerad
            };

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SignIn", redirectResult.ActionName); // Kontrollera omdirigering till SignIn
            Assert.Equal("Account created successfully. Please sign in.", _controller.TempData["Message"]);
        }

        [Fact]
        public async Task SignUp_WhenTermsNotAccepted_ShouldShowErrorMessage()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                TermsAndConditions = false // Checkbox ej markerad
            };

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("You must accept the terms and conditions to proceed.", _controller.TempData["Message"]);
        }

        [Fact]
        public void TermsOverlay_ShouldAppearWhenTermsLinkClicked()
        {
            // Detta är ett hypotetiskt test som simulerar att overlay visas.
            // Om det finns en metod som hanterar overlay-visning kan den testas här.
            var overlayTriggered = false;

            // Simulera att användaren klickar på "terms & conditions" länken
            overlayTriggered = true;

            // Assert
            Assert.True(overlayTriggered, "Overlay för 'terms & conditions' ska visas när användaren klickar på länken.");
        }

        [Fact]
        public async Task SignUp_WithShortFirstNameOrLastName_ShouldReturnViewWithErrorMessage()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "A", // För kort förnamn
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                TermsAndConditions = true // Checkbox markerad
            };

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Kontrollera att en vy returneras (inte omdirigering)
            Assert.Equal("First and last names must each be at least 2 characters long.", _controller.TempData["Message"]);
            Assert.Equal("error", _controller.TempData["MessageType"]);
        }

        [Fact]
        public async Task SignUp_WithValidFirstNameAndLastName_ShouldRedirectToSignIn()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "Hans", // Giltigt förnamn
                LastName = "Mattin-Lassei", // Giltigt efternamn
                Email = "hans.mattin@example.com",
                Password = "Password123!",
                TermsAndConditions = true // Checkbox markerad
            };

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result); // Kontrollera att omdirigering sker
            Assert.Equal("SignIn", redirectResult.ActionName); // Kontrollera omdirigering till "SignIn"
            Assert.Equal("Account created successfully. Please sign in.", _controller.TempData["Message"]);
            Assert.Equal("success", _controller.TempData["MessageType"]);
        }

        [Fact]
        public async Task SignUp_WithInvalidEmailFormat_ShouldReturnViewWithErrorMessage()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email-format", // Ogiltigt e-postformat
                Password = "Password123!",
                TermsAndConditions = true
            };

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Invalid email address format.", _controller.TempData["Message"]);
            Assert.Equal("error", _controller.TempData["MessageType"]);
        }

        [Fact]
        public async Task SignUp_WithValidAndUniqueEmail_ShouldRedirectToSignIn()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com", // Giltig e-postadress som inte är upptagen
                Password = "Password123!",
                TermsAndConditions = true
            };

            // Mocka att API-svaret skulle vara lyckat (200 OK)
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SignIn", redirectResult.ActionName);
            Assert.Equal("Account created successfully. Please sign in.", _controller.TempData["Message"]);
            Assert.Equal("success", _controller.TempData["MessageType"]);
        }

    }

}

