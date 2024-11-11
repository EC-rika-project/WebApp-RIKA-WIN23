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
                ProfileImageUrl = string.IsNullOrEmpty(userDto.ProfileImageUrl) ? "/images/Profilepic.jpg" : userDto.ProfileImageUrl,
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
                ProfileImageUrl = userDto.ProfileImageUrl,
                Age = userDto.Age,
                Gender = userDto.Gender
            };
            return View(viewModel);
        }


        //[HttpPost]
        //[Route("/Profile/Settings")]
        //public async Task<IActionResult> UpdateProfile(ProfilePageViewModel viewModel)
        //{
        //    var token = Request.Cookies["AuthToken"];

        //    if (string.IsNullOrEmpty(token))
        //    {
        //        // Om AuthToken saknas, omdirigera till inloggningssidan
        //        return Redirect("/signin");
        //    }

        //    var handler = new JwtSecurityTokenHandler();
        //    var jwtToken = handler.ReadJwtToken(token);
        //    var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;

        //    if (ModelState.IsValid)
        //    {

        //        if (viewModel.ProfileImage != null && viewModel.ProfileImage.Length > 0)
        //        {

        //            var fileName = $"{userId}_{Path.GetFileName(viewModel.ProfileImage.FileName)}";
        //            var filePath = Path.Combine("wwwroot/images/uploads", fileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await viewModel.ProfileImage.CopyToAsync(stream);
        //            }

        //            viewModel.ProfileImageUrl = $"/images/uploads/{fileName}";
        //        } 

        //        else
        //        {
        //            viewModel.ProfileImageUrl = string.IsNullOrEmpty(viewModel.ProfileImageUrl)
        //                            ? "/images/Profilepic.jpg"
        //                            : viewModel.ProfileImageUrl;
        //        }


        //        var userDto = new UserDto
        //        {
        //            UserId = userId!,
        //            FirstName = viewModel.FirstName,
        //            LastName = viewModel.LastName,
        //            Email = viewModel.Email,
        //            ProfileImageUrl = viewModel.ProfileImageUrl,
        //            Gender = viewModel.Gender,
        //            Age = viewModel.Age
        //        };


        //        var result = await _userService.UpdateUserAsync(userDto);
        //        if (result)
        //        {
        //            TempData["Message"] = "Profile updated successfully!";
        //            TempData["MessageType"] = "success";
        //        }
        //        else
        //        {
        //            TempData["Message"] = "Failed to update profile";
        //            TempData["MessageType"] = "error";
        //        }

        //    }
        //    return View("Settings", viewModel);
        //}


        [HttpPost]
        [Route("/Profile/Settings")]
        public async Task<IActionResult> UpdateProfile(ProfilePageViewModel viewModel)
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Redirect to signin page if AuthToken is missing
                return Redirect("/signin");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;

            if (ModelState.IsValid)
            {
                // Preserve the current ProfileImageUrl if no new image is uploaded
                string currentProfileImageUrl = viewModel.ProfileImageUrl ?? "/images/Profilepic.jpg";

                if (viewModel.ProfileImage != null && viewModel.ProfileImage.Length > 0)
                {
                    try
                    {
                        // Create a unique file name for the new image
                        var fileName = $"{userId}_{Path.GetFileName(viewModel.ProfileImage.FileName)}";
                        var filePath = Path.Combine("wwwroot/images/uploads", fileName);

                        // Ensure the directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await viewModel.ProfileImage.CopyToAsync(stream);
                        }

                        // Set the new ProfileImageUrl after a successful upload
                        viewModel.ProfileImageUrl = $"/images/uploads/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        // Handle image upload error (fallback to existing image)
                        TempData["Message"] = $"Error uploading image: {ex.Message}";
                        TempData["MessageType"] = "error";
                        viewModel.ProfileImageUrl = currentProfileImageUrl; // Fallback to existing image
                    }
                }
                else
                {
                    // Use the existing ProfileImageUrl if no new image is uploaded
                    viewModel.ProfileImageUrl = currentProfileImageUrl;
                }

                var userDto = new UserDto
                {
                    UserId = userId!,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    ProfileImageUrl = viewModel.ProfileImageUrl,
                    Gender = viewModel.Gender,
                    Age = viewModel.Age
                };

                var result = await _userService.UpdateUserAsync(userDto);
                if (result)
                {
                    TempData["Message"] = "Profile updated successfully!";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("Settings"); // Redirect to reload fresh data
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


        [Authorize]
        [Route("/Profile/Shipping")]
        public async Task<IActionResult> Shipping()
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

            var userAddress = await _userService.GetUserShippingInfoFromApiAsync(userId);

            var profilePageViewModel = new ProfilePageViewModel
            {
                UserId = userId,
                ProfileImageUrl = userDto.ProfileImageUrl,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                UserAddress = userAddress
            };

            return View(profilePageViewModel);
        }


        [HttpPost]
        [Route("/Profile/Shipping")]
        public async Task<IActionResult> UpdateShippingInfo(ProfilePageViewModel viewModel)
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Redirect to signin page if AuthToken is missing
                return Redirect("/signin");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;

            if (ModelState.IsValid)
            {

                var userAddressDto = new UserAddressDto
                {
                    UserId = userId!,
                    AddressLine = viewModel.UserAddress!.AddressLine,
                    ApartmentNumber = viewModel.UserAddress.ApartmentNumber,
                    PostalCode = viewModel.UserAddress.PostalCode,
                    City = viewModel.UserAddress.City,
                    Country = viewModel.UserAddress?.Country

                };

                var result = await _userService.UpdateUserShippingInfo(userAddressDto);
                if (result)
                {
                    TempData["Message"] = "Shipping adress updated successfully!";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("Shipping"); // Redirect to reload fresh data
                }
                else
                {
                    TempData["Message"] = "Failed to update shipping information";
                    TempData["MessageType"] = "error";
                }
            }

            return View("Shipping", viewModel);
        }


        [Route("/UnderConstruction")]
        public IActionResult UnderConstruction()
        {
            return View();
        }


    }


    
}

