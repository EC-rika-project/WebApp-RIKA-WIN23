using Infrastructure.Dtos;

namespace Infrastructure.Interfaces;

public interface IAppAuthenticationService
{
    Task<string?> SignInAsync(SignInDto viewModel);
    Task SignInUserWithTokenAsync(string token, bool rememberMe);
}
