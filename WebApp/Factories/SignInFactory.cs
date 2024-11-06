using Infrastructure.Dtos;
using WebApp.ViewModels;

namespace WebApp.Factories;

public class SignInFactory
{
    public static SignInDto CreateSignInDto(SignInViewModel viewModel)
    {
        return new SignInDto
        {
            Email = viewModel.Email,
            Password = viewModel.Password,
            RememberMe = viewModel.RememberMe
        };
    }
}
