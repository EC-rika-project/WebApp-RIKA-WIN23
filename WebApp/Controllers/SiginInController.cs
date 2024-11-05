using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;
using WebApp.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Infrastructure.Interfaces;
using WebApp.Factories;

namespace WebApp.Controllers;

public class SignInController(IAppAuthenticationService authService) : Controller
{
    private readonly IAppAuthenticationService _authService = authService;

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
}
