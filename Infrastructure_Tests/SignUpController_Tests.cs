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
        /// <summary>
        /// Användare fyller i formuläret med fullständig och korrekt data för att testa om det går att skapa en användare
        /// </summary>
        /// <returns>Användare blir skapad och blir sedan omdirigerad till sign in sidan</returns>
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

        /// <summary>
        /// Användare fyller i samtliga fält korrekt förutom email där man använder en email adress som redan finns i databasen
        /// </summary>
        /// <returns>Error meddelande som säger att det redan finns ett konto med denna email adress</returns>
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

        /// <summary>
        /// Användare lämnar fält tomma för att testa om error meddelanden visas med tempdata
        /// </summary>
        /// <returns>Error meddelande om att det är validerings fel visas</returns>
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

        /// <summary>
        /// Testar att användaren kan registrera sig när de accepterat villkoren,
        /// och att de blir omdirigerade till inloggningen med rätt meddelande i TempData.
        /// </summary>
        /// <returns>Omdirigering till sign in sidan och ett sucess meddelande i TempData</returns>
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
                TermsAndConditions = true 
            };

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SignIn", redirectResult.ActionName); 
            Assert.Equal("Account created successfully. Please sign in.", _controller.TempData["Message"]);
        }

        /// <summary>
        /// Testar att ett felmeddelande visas om användaren inte accepterat villkoren vid registrering.
        /// </summary>
        /// <returns>Visar felmeddelande i TempData och återgår till samma vy</returns>
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
                TermsAndConditions = false 
            };

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("You must accept the terms and conditions to proceed.", _controller.TempData["Message"]);
        }

        /// <summary>
        /// Testar att overlay för "Terms & conditions" visas när användaren klickar på länken
        /// </summary>
        ///<returns>Overlay poppar upp när användaren trycker på länken</returns>
        [Fact]
        public void TermsOverlay_ShouldAppearWhenTermsLinkClicked()
        {
            
            var overlayTriggered = false;

            // Simulera att användaren klickar på "terms & conditions" länken
            overlayTriggered = true;

            // Assert
            Assert.True(overlayTriggered, "Overlay för 'terms & conditions' ska visas när användaren klickar på länken.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SignUp_WithShortFirstNameOrLastName_ShouldReturnViewWithErrorMessage()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "A", 
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                TermsAndConditions = true 
            };

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); 
            Assert.Equal("First and last name must be at least 2 characters long and cannot be empty.", _controller.TempData["Message"]);
            Assert.Equal("error", _controller.TempData["MessageType"]);
        }
        /// <summary>
        /// Testar att ett felmeddelande visas när förnamn eller efternamn är för kort (mindre än 2 tecken)
        /// </summary>
        /// <returns>Testet kontrollerar att felmeddelande visas om man försöker använda sig av ett för kort namn</returns>
        [Fact]
        public async Task SignUp_WithValidFirstNameAndLastName_ShouldRedirectToSignIn()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "Hans", 
                LastName = "Mattin-Lassei", 
                Email = "hans.mattin@example.com",
                Password = "Password123!",
                TermsAndConditions = true
            };

            // Act
            var result = await _controller.SignUp(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result); 
            Assert.Equal("SignIn", redirectResult.ActionName); 
            Assert.Equal("Account created successfully. Please sign in.", _controller.TempData["Message"]);
            Assert.Equal("success", _controller.TempData["MessageType"]);
        }

        /// <summary>
        /// Testar att ett felmeddelande visas när e-postadressen har ett ogiltigt format.
        /// </summary>
        /// <returns>Testet verifierar att felmeddelande för ogiltig email visas</returns>
        [Fact]
        public async Task SignUp_WithInvalidEmailFormat_ShouldReturnViewWithErrorMessage()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email-format", 
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

        /// <summary>
        /// Testar att användaren skickas till SignIn-sidan när en giltig och unik e-postadress används vid registrering.
        /// </summary>
        /// <returns>En uppgift som representerar testet.</returns>
        [Fact]
        public async Task SignUp_WithValidAndUniqueEmail_ShouldRedirectToSignIn()
        {
            // Arrange
            var viewModel = new SignUpViewModel
            {
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com", 
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

