using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebApp.ViewModels;
namespace WebApp.Controllers;


public class SignUpController(IHttpClientFactory httpClientFactory, IConfiguration configuration) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IConfiguration _configuration = configuration;

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

        var securityKey = _configuration["SecurityKeys:WebAppKey"];

        var dto = new SignUpDto
        {
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            Email = viewModel.Email,
            Password = viewModel.Password,
            SecurityKey = securityKey

        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsync("https://localhost:7163/api/SignUp", jsonContent);


        if (!response.IsSuccessStatusCode)
        {
            TempData["Message"] = "User with the same Email address already exists. ";
            TempData["MessageType"] = "error";

            return View(viewModel);
        }

        TempData["Message"] = "Account created successfully. Please sign in.";
        TempData["MessageType"] = "success";

        return RedirectToAction("SignIn", "SignIn");
     
    }
}


