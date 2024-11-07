using Infrastructure.Models;
using Newtonsoft.Json;

namespace WebApp.ViewModels
{


    public class ProfilePageViewModel
    {
        public string PageTitle { get; set; } = "Profile";

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int Age { get; set; }

        public string? Gender { get; set; }
    }
}
