using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService
    {
        public UserModel GetFakeUser()
        {
            return new UserModel
            {
                UserId = "1",
                Name = "Harre Birger Svenning",
                Email = "Harreking@gmail.com",
                Age = 85,
                Gender = "Male",
                ProfileImage = "/images/Profilepic.jpg"
            };
        }
    }
}
