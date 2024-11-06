using System.Security.Claims;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using WebApp.Factories;

namespace WebApp.Controllers
{
    public class SignInController : Controller
    {
        private readonly IAppAuthenticationService _authService;

        public SignInController(IAppAuthenticationService authService)
        {
            _authService = authService;
        }

        [Route("/signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [Route("/signin")]
        public async Task<IActionResult> SignIn(SignInViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                var signInDto = SignInFactory.CreateSignInDto(viewModel);
                var token = await _authService.SignInAsync(signInDto);

                if (token != null)
                {
                    await _authService.SignInUserWithTokenAsync(token, signInDto.RememberMe);
                    TempData["Message"] = "Successfully logged in!";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("Index", "Home");
                }

                TempData["Message"] = "Login failed. Please try again.";
                TempData["MessageType"] = "error";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"An error occurred: {ex.Message}";
                TempData["MessageType"] = "error";
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("signin-google")]
        public IActionResult GoogleSignIn()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback))
            };
            Console.WriteLine("Redirect URI: " + properties.RedirectUri);

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                TempData["Message"] = "Google login failed";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(SignIn));
            }

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;

            try
            {
                var signInDto = new SignInDto
                {
                    Email = email,
                    ExternalProvider = "Google",
                    ExternalProviderId = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                };

                var token = await _authService.SignInWithExternalProviderAsync(signInDto);

                if (token != null)
                {
                    await _authService.SignInUserWithTokenAsync(token, true);
                    TempData["Message"] = "Successfully logged in with Google!";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("Index", "Home");
                }

                TempData["Message"] = "Failed to Google login";
                TempData["MessageType"] = "error";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error {ex.Message}";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction(nameof(SignIn));
        }
    }
}
