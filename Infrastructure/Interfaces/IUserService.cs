using Infrastructure.DTOs;
using Infrastructure.Models;

public interface IUserService
{
    UserModel GetUserProfileByIdAsync(string userId);
    ControllerResultModel UpdateUserProfileAsync(UserDto userDto);

    Task<bool> UpdateUserAsync(UserDto userDto);

}

