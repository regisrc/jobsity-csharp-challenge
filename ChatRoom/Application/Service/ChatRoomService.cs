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
        private readonly IUserRepository _userRepository;

        public ChatRoomService(ILogger<ChatRoomService> logger, IChatRoomEventPublisher publisher, IChatRoomRepository chatRoomRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _publisher = publisher;
            _chatRoomRepository = chatRoomRepository;
            _userRepository = userRepository;
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
            var user = await _userRepository.GetById(chatRoomEvent.CreatorId);

            if (user == null)
            {
                _logger.LogInformation("User not found");

                return;
            }

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

        public async Task<List<ChatRoomDto>> GetChatRooms()
        {
            var result = await _chatRoomRepository.GetAll();

            return result.Select(x => new ChatRoomDto
            {
                CreatorId = x.CreatorId,
                Name = x.Name,
                Id = x.Id
            }).ToList();
        }
    }
}
