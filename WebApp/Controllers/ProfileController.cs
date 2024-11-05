using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebApp.ViewModels;

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
        public async Task<IActionResult> Index()
        {
            var cookie = Request.Cookies["UserCookie"];
            if (!string.IsNullOrEmpty(cookie))
            {
                // Försök att deserialisera kakan till ProfilePageViewModel
                var user = JsonConvert.DeserializeObject<ProfilePageViewModel>(cookie);
                if (user != null)
                {
                    return View(user);
                }
            }

            // Hämta användardata från API eller skapa en "fake" användare
            UserModel userModel = _userService.GetFakeUser(); // Anta att du har en metod för att hämta användaren
            if (userModel != null)
            {
                // Mappa UserModel till ProfilePageViewModel
                var viewModel = new ProfilePageViewModel
                {
                    Name = userModel.Name,
                    Email = userModel.Email,
                    ProfileImage = userModel.ProfileImage
                };

                return View(viewModel);
            }

            // Om ingen användare hittades, hantera det här (exempelvis redirecta till inloggning)
            return RedirectToAction("Login", "Account");
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

        [Route("/Profile/Settings")]
        public IActionResult Settings()
        {
            var cookie = Request.Cookies["UserCookie"];
            if (!string.IsNullOrEmpty(cookie))
            {
                // Försök att deserialisera kakan till ProfilePageViewModel
                var user = JsonConvert.DeserializeObject<ProfilePageViewModel>(cookie);
                if (user != null)
                {
                    return View(user);
                }
            }

            // Hämta användardata från API eller skapa en "fake" användare
            UserModel userModel = _userService.GetFakeUser(); // Anta att du har en metod för att hämta användaren
            if (userModel != null)
            {
                // Mappa UserModel till ProfilePageViewModel
                var viewModel = new ProfilePageViewModel
                {
                    Name = userModel.Name,
                    Email = userModel.Email,
                    ProfileImage = userModel.ProfileImage,
                    Age = userModel.Age,
                    Gender = userModel.Gender
                };

                return View(viewModel);
            }
            return RedirectToAction("Index");
        }
    }
}
