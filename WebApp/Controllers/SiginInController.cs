using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using WebApp.Factories;
using Microsoft.AspNetCore.Authentication.Facebook;

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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback", "SignIn", null, Request.Scheme)
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("GoogleCallback")]
        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    TempData["Message"] = "Failed to Google login";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(SignIn));
                }

                var user = User.Identity.Name;
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                TempData["Message"] = "Successfully logged in with Google!";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error {ex.Message}";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction(nameof(SignIn));
        }

        [HttpGet]
        [Route("signin-facebook")]
        public IActionResult FacebookSignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookCallback", "SignIn", null, Request.Scheme)
            };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [Route("FacebookCallback")]
        public async Task<IActionResult> FacebookCallback()
        {
            Console.WriteLine("FacebookCallback: Start of method.");

            try
            {
                if (User?.Identity?.IsAuthenticated != true)
                {
                    TempData["Message"] = "Facebook login failed";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(SignIn));
                }

                var user = User.Identity.Name;
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                TempData["Message"] = "Successfully logged in with Facebook!";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction(nameof(SignIn));
        }
    }
}
