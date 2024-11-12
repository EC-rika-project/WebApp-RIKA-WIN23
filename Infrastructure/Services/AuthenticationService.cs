using Infrastructure.Dtos;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationProperties = Microsoft.AspNetCore.Authentication.AuthenticationProperties;

namespace Infrastructure.Services;
public class AuthenticationService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IAppAuthenticationService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly string _apiKey = configuration["ApiKey"]!;

    public async Task<string?> SignInAsync(SignInDto signInDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(signInDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"https://userprovider-rika-win23.azurewebsites.net/api/SignIn?key={_apiKey}", content);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var jwt = JsonConvert.DeserializeObject<JwtDto>(json);
            return jwt?.JWT;
        }
        return null;
    }

    public async Task SignInUserWithTokenAsync(string token, bool rememberMe)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var claimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims, "jwt");
        var principal = new ClaimsPrincipal(claimsIdentity);

        await _httpContextAccessor.HttpContext!.SignInAsync(
           CookieAuthenticationDefaults.AuthenticationScheme,
           principal,
           new AuthenticationProperties
           {
               IsPersistent = rememberMe,
               ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
           });

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddMinutes(30)
        };

        _httpContextAccessor.HttpContext!.Response.Cookies.Append("AuthToken", token, cookieOptions);
    }
}
