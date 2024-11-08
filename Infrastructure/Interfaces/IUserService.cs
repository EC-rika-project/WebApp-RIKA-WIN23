using Infrastructure.DTOs;
using Infrastructure.Models;

public interface IUserService
{
    UserDto GetUserProfileByIdAsync(string userId);
    ControllerResultModel UpdateUserProfileAsync(UserDto userDto);

    Task<bool> UpdateUserAsync(UserDto userDto);

}

