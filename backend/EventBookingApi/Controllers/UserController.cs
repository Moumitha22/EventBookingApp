using EventBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IApiResponseMapper _responseMapper;

        public UserController(IUserService userService, IApiResponseMapper responseMapper)
        {
            _userService = userService;
            _responseMapper = responseMapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(_responseMapper.MapToOkResponse("User fetched successfully", user));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(_responseMapper.MapToOkResponse("All users fetched successfully", users));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(_responseMapper.MapToOkResponse("User deleted successfully"));
        }
    }
}
