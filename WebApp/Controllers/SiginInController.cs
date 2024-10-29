using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class SignInController : Controller
{
    [Route("/signin")]
    public IActionResult SignIn()
    {
        return View();
    }
}
