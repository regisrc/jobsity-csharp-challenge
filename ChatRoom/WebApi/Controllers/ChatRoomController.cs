using Application.Dto;
using Application.Interface;
using Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatroomController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IChatRoomService _chatRoomService;

        public ChatroomController(IChatRoomService chatRoomService, IUserService userService)
        {
            _chatRoomService = chatRoomService;
            _userService = userService;
        }

        [HttpPost(Name = "Create Chatroom")]
        public async Task<IActionResult> PostChatroom([FromBody]ChatRoomDto chatRoomDto, Guid? token)
        {
            var result = await _userService.VerifyToken(token);

            if (result)
                _chatRoomService.CreateChatRoom(chatRoomDto);

            return Ok();
        }

        [HttpGet(Name = "Get Chatrooms")]
        public async Task<IActionResult> GetChatrooms(Guid? token)
        {
            var result = await _userService.VerifyToken(token);

            if (result)
                return Ok(await _chatRoomService.GetChatRooms());

            return Ok();
        }
    }
}