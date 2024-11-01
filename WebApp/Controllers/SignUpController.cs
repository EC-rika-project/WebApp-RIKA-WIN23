using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebApp.ViewModels;
namespace WebApp.Controllers;


public class SignUpController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SignUpController(IHttpClientFactory httpClientFactory)
    {
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
            return View(viewModel);
        }

        var dto = new SignUpDto
        {
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            Email = viewModel.Email,
            Password = viewModel.Password
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsync("https://localhost:7163/api/signup", jsonContent);


        if (!response.IsSuccessStatusCode)
        {
            TempData["Message"] = "User with the same Email address already exists. ";
            TempData["MessageType"] = "error";
            return View(viewModel);
        }

        TempData["Message"] = "Account created successfully. Please sign in.";
        TempData["MessageType"] = "success";
        return View(viewModel);
        //RedirectToAction("SignIn", "SignIn");
    }
}


