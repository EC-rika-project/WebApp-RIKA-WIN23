namespace Infrastructure.Dtos;

public class ResetPasswordDto
{
    public string JWT { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
