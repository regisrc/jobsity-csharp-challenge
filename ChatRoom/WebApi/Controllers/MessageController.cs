using Application.Dto;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost(Name = "Send Message")]
        public IActionResult PostMessage(MessageDto messageDto)
        {
            _messageService.PublishMessage(messageDto);

            return Ok();
        }
    }
}