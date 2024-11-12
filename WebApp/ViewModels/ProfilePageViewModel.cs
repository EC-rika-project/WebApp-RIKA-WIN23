using Infrastructure.DTOs;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{


    public class ProfilePageViewModel
    {
        public string PageTitle { get; set; } = "Profile";

        [ProtectedPersonalData]
        public string UserId { get; set; }

        [ProtectedPersonalData]
        public string FirstName { get; set; }

        [ProtectedPersonalData]
        public string LastName { get; set; }

        [ProtectedPersonalData]
        public string Email { get; set; }
        [ProtectedPersonalData]

        public string? ProfileImageUrl { get; set; }

        [ProtectedPersonalData]
        public int Age { get; set; }

        [ProtectedPersonalData]
        public string? Gender { get; set; }

        public IFormFile? ProfileImage { get; set; }

       
        public UserAddressDto? UserAddress { get; set; }
    }
}
