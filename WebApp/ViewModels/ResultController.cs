//using Infrastructure.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//[ApiController]
//[Route("api/[controller]")]
//public class AccountController : ControllerBase
//{
//    private readonly IUserService _userService;

//    public AccountController(IUserService userService)
//    {
//        _userService = userService;
//    }

//    // GET: api/account/{userId}
//    [HttpGet("{userId}")]
//    public async Task<IActionResult> GetUserProfile(string userId)
//    {
//        var userProfile = await _userService.GetUserProfileAsync(userId);
//        if (userProfile == null)
//        {
//            return NotFound("User not found");
//        }
//        return Ok(userProfile);
//    }

//    // POST: api/account/update
//    [HttpPost("update")]
//    public async Task<IActionResult> UpdateUserProfile([FromBody] UserModel userModel)
//    {
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(ModelState);
//        }

//        bool updateSuccessful = await _userService.UpdateUserProfileAsync(userModel);
//        if (updateSuccessful)
//        {
//            return Ok("Update successful");
//        }
//        return BadRequest("Update failed");
//    }
//}
