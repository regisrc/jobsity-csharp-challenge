using Application.Dto;
using Application.Event;
using Application.Interface;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;

namespace Application.Service
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly ILogger _logger;
        private readonly IChatRoomEventPublisher _publisher;

        public ChatRoomService(ILogger<ChatRoomService> logger, IChatRoomEventPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        public void CreateChatRoom(ChatRoomDto chatRoomDto)
        {
            _logger.LogInformation("Publish Message Event");

            var @event = new ChatRoomEvent
            {
                Guid = Guid.NewGuid(),
                CreatorId = chatRoomDto.CreatorId,
                Name = chatRoomDto.Name
            };

            _publisher.Publish(@event);
        }
    }
}
