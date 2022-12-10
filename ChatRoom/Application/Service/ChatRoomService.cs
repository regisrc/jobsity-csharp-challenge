using Application.Dto;
using Application.Event;
using Application.Interface;
using Infrastructure.Entity;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;

namespace Application.Service
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly ILogger _logger;
        private readonly IChatRoomEventPublisher _publisher;
        private readonly IChatRoomRepository _chatRoomRepository;

        public ChatRoomService(ILogger<ChatRoomService> logger, IChatRoomEventPublisher publisher, IChatRoomRepository chatRoomRepository)
        {
            _logger = logger;
            _publisher = publisher;
            _chatRoomRepository = chatRoomRepository;
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

        public async Task SaveChatRoom(ChatRoomEvent chatRoomEvent) 
        {
            var entity = new ChatRoomEntity
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Name = chatRoomEvent.Name,
                CreatorId = chatRoomEvent.CreatorId
            };

            await _chatRoomRepository.Add(entity);

            _logger.LogInformation("ChatRoom saved");
        }
    }
}
