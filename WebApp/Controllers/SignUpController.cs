using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
namespace WebApp.Controllers;

public class SignUpController : Controller
{
    [HttpGet]
    [Route("/signup")]
    public IActionResult SignUp()
    {
        return View();
    }
    [HttpPost]
    [Route("/signup")]
    public IActionResult SignUp(SignUpViewModel viewModel)
    {
        return View();
    }
}
