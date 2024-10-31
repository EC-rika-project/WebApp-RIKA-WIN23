using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ProfileController : Controller
    {

        [Route("/Profile")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("/Profile/Privacy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }


        [Route("/Profile/FAQ")]
        public IActionResult Faqs()
        {
            return View();
        }
    }
}
