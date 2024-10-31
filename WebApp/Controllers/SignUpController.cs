using Infrastructure.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebApp.ViewModels;
namespace WebApp.Controllers;

public class SignUpController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpClientFactory _httpClientFactory;

    public SignUpController(UserManager<IdentityUser> userManager, IHttpClientFactory httpClientFactory)
    {
        _userManager = userManager;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    [Route("/signup")]
    public IActionResult SignUp()
    {
        return View();
    }


    [HttpPost]
    [Route("/signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Var vänlig och korrigera de markerade felen.";
            return View(viewModel);
        }

        var exists = await _userManager.FindByEmailAsync(viewModel.Email);
        if (exists != null)
        {
            TempData["Error"] = "Användare med denna e-postadress finns redan.";
            return View(viewModel);
        }

        var user = new IdentityUser
        {
            UserName = viewModel.UserName,
            Email = viewModel.Email
        };


        var result = await _userManager.CreateAsync(user, viewModel.Password);
        if (result.Succeeded)
        {

            var dto = new SignUpDto
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                Password = viewModel.Password
            };


            var jsonContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync("", jsonContent); //lägg till api länk

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Användaren har skapats framgångsrikt.";
                return RedirectToAction("SignIn", "SignIn");
            }


            var errorContent = await response.Content.ReadAsStringAsync();
            TempData["Error"] = $"Kunde inte skicka data till API:t: {errorContent}";
            return View(viewModel);
        }
            foreach (var error in result.Errors)
            {
                TempData["Error"] = error.Description; 
            }
        return View(viewModel);
    }
}
