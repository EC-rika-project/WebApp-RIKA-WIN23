using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
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

        [Authorize]
        [Route("/Profile")]
        public async Task<IActionResult> Index()
        {

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Om AuthToken saknas, omdirigera till inloggningssidan
                return Redirect("/signin");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;

            var userDto = await _userService.GetUserFromApiAsync(userId);

            var viewModel = new ProfilePageViewModel
            {
                UserId = userId,
                FirstName = userDto.FirstName!,
                LastName = userDto.LastName!,
                Email = userDto.Email,
                ProfileImageUrl = string.IsNullOrEmpty(userDto.ProfileImageUrl) ? userDto.ProfileImageUrl : "/images/Profilepic.jpg",
            };

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


        [Authorize]
        [Route("/Profile/Settings")]
        public async Task<IActionResult> Settings()
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Om AuthToken saknas, omdirigera till inloggningssidan
                return Redirect("/signin");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);


            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;

            var userDto = await _userService.GetUserFromApiAsync(userId);

            var viewModel = new ProfilePageViewModel
            {
                UserId = userId,
                FirstName = userDto.FirstName!,
                LastName = userDto.LastName!,
                Email = userDto.Email,
                ProfileImageUrl = string.IsNullOrEmpty(userDto.ProfileImageUrl) ? userDto.ProfileImageUrl : "/images/Profilepic.jpg",
                Age = userDto.Age,
                Gender = userDto.Gender
            };
            return View(viewModel);
        }


        [HttpPost]
        [Route("/Profile/Settings")]
        public async Task<IActionResult> UpdateProfile(ProfilePageViewModel viewModel)
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Om AuthToken saknas, omdirigera till inloggningssidan
                return Redirect("/signin");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);


            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;

            if (ModelState.IsValid)
            {
                var userDto = new UserDto
                {
                    UserId = userId,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    ProfileImageUrl = string.IsNullOrEmpty(viewModel.ProfileImageUrl)? "/images/Profilepic.jpg": viewModel.ProfileImageUrl,
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
                }

            }
            return View("Settings", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [Route("/UnderConstruction")]
        public IActionResult UnderConstruction()
        {
            return View();
        }


    }


    
}

