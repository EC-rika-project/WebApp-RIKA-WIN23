using Infrastructure.DTOs;
using Infrastructure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService
    {

        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDto> GetUserFromApiAsync(string userId)
        {
            
            var response = await _httpClient.GetAsync($"https://localhost:7163/api/Profile?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                // Om anropet var framgångsrikt, deserialisera svaret
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserDto>(content);
            }

            // Om anropet misslyckades, returnera null eller hantera fel på något sätt
            return null!;
        }


        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("https://localhost:7163/api/Profile", content);

            //if (response.IsSuccessStatusCode)
            //{
            //    return true;
            //}
            //return false;
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error or pass it up to the calling method for display
                Console.WriteLine($"API Error: {errorContent}");
                return false;
            }

            return true;
        }




        public UserDto GetFakeUser()
        {
            return new UserDto
            {
                UserId = "1",
                FirstName = "Harre",
                LastName = "Birger Svenning",
                Email = "Harreking@gmail.com",
                Age = 85,
                Gender = "Male",
                ProfileImageUrl = "/images/Profilepic.jpg"
            };
        }
    }
}
