using Infrastructure.Models;

public interface IUserService
{
    UserModel GetUserProfileByIdAsync(string userId);
    ControllerResultModel UpdateUserProfileAsync(UserModel userModel);
}

