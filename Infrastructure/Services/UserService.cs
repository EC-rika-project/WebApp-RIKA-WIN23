﻿using Infrastructure.DTOs;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public UserService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<UserDto> GetUserFromApiAsync(string userId)
        {
            var apiKey = _configuration!.GetSection("ApiKey")["Secret"];


            var response = await _httpClient.GetAsync($"https://userprovider-rika-win23.azurewebsites.net/api/Profile/?key={apiKey}&userId={userId}");

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
            var apiKey = _configuration!.GetSection("ApiKey")["Secret"];

            var content = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://userprovider-rika-win23.azurewebsites.net/api/Profile/?key={apiKey}", content);

            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error or pass it up to the calling method for display
                Console.WriteLine($"API Error: {errorContent}");
                return false;
            }

            return true;
        }

    }
}
