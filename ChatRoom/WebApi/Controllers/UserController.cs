using Application.Dto;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var result = await _userService.Login(userDto);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            await _userService.CreateUser(userDto);

            return Ok();
        }
    }
}