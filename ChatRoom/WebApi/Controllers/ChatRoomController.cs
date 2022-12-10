using Application.Dto;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatroomController : ControllerBase
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatroomController(IChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        [HttpPost(Name = "Create Chatroom")]
        public IActionResult PostChatroom(ChatRoomDto chatRoomDto)
        {
            _chatRoomService.CreateChatRoom(chatRoomDto);

            return Ok();
        }
    }
}