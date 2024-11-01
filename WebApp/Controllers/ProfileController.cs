using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ProfileController : Controller
    {

        private readonly UserService _userService;

        public ProfileController(UserService userService)
        {
            _userService = userService;
        }

        [Route("/Profile")]
        public IActionResult Index()
        {

            UserModel fakeUser = _userService.GetFakeUser();

            return View(fakeUser);
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
