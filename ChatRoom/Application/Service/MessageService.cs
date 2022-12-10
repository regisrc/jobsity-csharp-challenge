using Application.Dto;
using Application.Event;
using Application.Interface;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;

namespace Application.Service
{
    public class MessageService : IMessageService
    {
        private readonly ILogger _logger;
        private readonly IPublisher _publisher;

        public MessageService(ILogger<MessageService> logger, IPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        public void PublishMessage(MessageDto messageDto)
        {
            _logger.LogInformation("Publish Message Event");

            var @event = new MessageEvent
            {
                Guid = Guid.NewGuid(),
                Message = messageDto.Message
            };

            _publisher.Publish(@event);
        }

        public void SaveMessage(MessageEvent messageEvent)
        {
            _logger.LogInformation(
                $"[new message | {DateTime.Now:yyyy-MM-dd HH:mm:ss}] " + messageEvent.Message);
        }
    }
}
