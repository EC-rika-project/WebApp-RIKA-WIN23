using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.ViewModels;


namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("/")]
        public IActionResult Index()
        {
            HomeIndexViewModel viewModel = new();
            return View(viewModel);            
                
        }
    }
}
