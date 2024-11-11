using Infrastructure.Dtos;
using WebApp.ViewModels;

namespace WebApp.Factories;

public static class ResetPasswordFactory
{
    public static ResetPasswordDto CreateResetPasswordDto(ResetPasswordViewModel viewModel)
    {
        return new ResetPasswordDto
        {
            JWT = viewModel.Token,
            NewPassword = viewModel.NewPassword
        };
    }
}
