using Application.Dto;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        [HttpPost(Name = "Send Message")]
        public async Task<IActionResult> PostMessage([FromBody]MessageCreateDto messageDto, Guid? token)
        {
            var result = await _userService.VerifyToken(token);

            if (result)
                _messageService.PublishMessage(messageDto);

            return Ok();
        }

        [HttpGet(Name = "Get Messages")]
        public async Task<IActionResult> GetMessages(Guid chatRoomId, Guid? token)
        {
            var result = await _userService.VerifyToken(token);

            if (result)
                return Ok(await _messageService.GetMessages(chatRoomId));

            return Ok();
        }
    }
}