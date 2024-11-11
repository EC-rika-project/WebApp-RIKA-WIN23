using Infrastructure.Dtos;
using WebApp.ViewModels;

namespace WebApp.Factories;

public static class ForgotPasswordFactory
{
    public static ForgotPasswordDto CreateForgotPasswordDto(ForgotPasswordViewModel viewModel)
    {
        return new ForgotPasswordDto
        {
            Email = viewModel.Email
        };
    }
}
