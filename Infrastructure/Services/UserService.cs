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

        public async Task<UserModel> GetUserFromApiAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("https://api.example.com/user");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserModel>(content);
            }

            return null!;
        }

        public UserModel GetFakeUser()
        {
            return new UserModel
            {
                UserId = "1",
                Name = "Harre Birger Svenning",
                Email = "Harreking@gmail.com",
                Age = 85,
                Gender = Gender.Male,
                ProfileImage = "/images/Profilepic.jpg"
            };
        }
    }
}
