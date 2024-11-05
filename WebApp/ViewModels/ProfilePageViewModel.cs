using Infrastructure.Models;

namespace WebApp.ViewModels
{


    public class ProfilePageViewModel
    {
        public string PageTitle { get; set; } = "Profile";

        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }
}
