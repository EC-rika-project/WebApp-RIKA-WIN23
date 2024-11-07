using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
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
                var user = JsonConvert.DeserializeObject<ProfilePageViewModel>(cookie);
                if (user != null)
                {
                    return View(user);
                }
            }

            //Tillfälligt, ska tas ifrån UserCookien.
            var userId = "a27b6c49-93dc-40d1-a01d-edb3c7e2101d";

            var userDto = await _userService.GetUserFromApiAsync(userId);


            var viewModel = new ProfilePageViewModel
            {
                FirstName = userDto.FirstName!,
                LastName = userDto.LastName!,
                Email = userDto.Email,
                ProfileImageUrl = userDto.ProfileImageUrl!,
            };
            Console.Write(viewModel);
            return View(viewModel);
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
        public async Task<IActionResult> Settings()
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
            //Tillfälligt, ska tas ifrån UserCookien.
            var userId = "a27b6c49-93dc-40d1-a01d-edb3c7e2101d";

            var userDto = await _userService.GetUserFromApiAsync(userId);


            var viewModel = new ProfilePageViewModel
            {
                UserId = userId,
                FirstName = userDto.FirstName!,
                LastName = userDto.LastName!,
                Email = userDto.Email,
                ProfileImageUrl = userDto.ProfileImageUrl!,
                Age = userDto.Age,
                Gender = userDto.Gender
            };
            return View(viewModel);
        }


        [HttpPost]
        [Route("/Profile/Settings")]
        public async Task<IActionResult> UpdateProfile(ProfilePageViewModel viewModel)
        {


            if (ModelState.IsValid)
            {
                var userDto = new UserDto
                {
                    UserId = "a27b6c49-93dc-40d1-a01d-edb3c7e2101d",
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    ProfileImageUrl = "/images/rika-profile-img.svg",
                    Gender = viewModel.Gender,
                    Age = viewModel.Age
                };
                var result = await _userService.UpdateUserAsync(userDto);
                if (result)
                {
                    TempData["Message"] = "Profile updated successfully!";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    TempData["Message"] = "Failed to update profile";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Settings");
                }

            }
            return View("Settings", viewModel);
        }
    }
}

