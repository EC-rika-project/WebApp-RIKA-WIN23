using Infrastructure.Dtos;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Services;

public class ResetPasswordService(HttpClient httpClient, IConfiguration configuration) : IResetPasswordService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _apiKey = configuration["ApiKey:Secret"]!;
    public async Task<JwtDto> InitiatePasswordResetAsync(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var getUrl = $"https://userprovider-rika-win23.azurewebsites.net/api/ResetToken/{forgotPasswordDto.Email}?key={_apiKey}";
            var response = await _httpClient.GetAsync(getUrl);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to retrieve reset token.");

            var tokenString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JwtDto>(tokenString) ??
                   throw new Exception("Invalid token response");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to initiate password reset: {ex.Message}");
        }
    }

    public async Task<bool> ValidateAndResetPasswordAsync(ResetPasswordDto resetDto)
    {
        try
        {
            var json = JsonConvert.SerializeObject(resetDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(
                $"https://userprovider-rika-win23.azurewebsites.net/api/ValidateReset?key={_apiKey}",
                content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to reset password: {ex.Message}");
        }
    }
}
