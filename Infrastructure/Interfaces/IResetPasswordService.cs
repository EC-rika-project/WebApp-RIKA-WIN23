using Infrastructure.Dtos;

namespace Infrastructure.Interfaces
{
    public interface IResetPasswordService
    {
        Task<JwtDto> InitiatePasswordResetAsync(ForgotPasswordDto forgotPasswordDto);
        Task<bool> ValidateAndResetPasswordAsync(ResetPasswordDto resetDto);
    }
}
