using Application.Event;
using Application.Interface;
using Microsoft.Extensions.Logging;

namespace Application.Service
{
    public class MessageService : IMessageService
    {
        private readonly ILogger _logger;

        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;
        }

        public void SaveMessage(MessageEvent messageEvent)
        {
            _logger.LogInformation(
                $"[new message | {DateTime.Now:yyyy-MM-dd HH:mm:ss}] " + messageEvent.Message);
        }
    }
}
