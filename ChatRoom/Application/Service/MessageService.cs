using Application.Dto;
using Application.Event;
using Application.Interface;
using Infrastructure.Entity;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;

namespace Application.Service
{
    public class MessageService : IMessageService
    {
        private readonly ILogger _logger;
        private readonly IMessageEventPublisher _publisher;
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IUserRepository _userRepository;

        public MessageService(
            ILogger<MessageService> logger,
            IMessageEventPublisher publisher,
            IMessageRepository messageRepository,
            IChatRoomRepository chatRoomRepository,
            IUserRepository userRepository)
        {
            _logger = logger;
            _publisher = publisher;
            _messageRepository = messageRepository;
            _chatRoomRepository = chatRoomRepository;
            _userRepository = userRepository;
        }

        public void PublishMessage(MessageCreateDto messageDto)
        {
            _logger.LogInformation("Publish Message Event");

            var @event = new MessageEvent
            {
                Guid = Guid.NewGuid(),
                Message = messageDto.Message,
                ChatRoomId = messageDto.ChatRoomId,
                UserId = messageDto.UserId
            };

            _publisher.Publish(@event);

            var botEvent = new MessageEvent
            {
                Guid = Guid.NewGuid(),
                Message = messageDto.Message,
                ChatRoomId = messageDto.ChatRoomId,
                UserId = Guid.Parse(Environment.GetEnvironmentVariable("bot_guid"))
            };

            _publisher.PublishBotMessage(botEvent);
        }

        public async Task SaveMessage(MessageEvent messageEvent)
        {
            var chatRoom = await _chatRoomRepository.GetById(messageEvent.ChatRoomId);

            if (chatRoom == null)
            {
                _logger.LogInformation("ChatRoom not found");

                return;
            }

            var user = await _userRepository.GetById(messageEvent.UserId);

            if (user == null && messageEvent.UserId != Guid.Parse(Environment.GetEnvironmentVariable("bot_guid")))
            {
                _logger.LogInformation("User not found");

                return;
            }

            var entity = new MessageEntity
            {
                Id = messageEvent.Guid,
                CreationDate = DateTime.UtcNow,
                ChatRoomId = messageEvent.ChatRoomId,
                Message = messageEvent.Message,
                UserId = messageEvent.UserId
            };

            await _messageRepository.Add(entity);

            _logger.LogInformation("Message saved");
        }

        public async Task<List<MessageDto>> GetMessages(Guid chatRoomId)
        {
            var result = await _messageRepository.GetByChatRoomId(chatRoomId);
            var users = await _userRepository.GetAll();

            if (!result.Any())
                return new List<MessageDto>();

            return result.Select(x => new MessageDto
            {
                ChatRoomId = x.ChatRoomId,
                Message = x.Message,
                UserId = x.UserId,
                Name = GetUserName(users, x.UserId)
            }).ToList();
        }

        public string GetUserName(List<UserEntity> users, Guid userId)
        {
            if (userId == Guid.Parse(Environment.GetEnvironmentVariable("bot_guid")))
                return "Bot";

            return users.Find(x => x.Id == userId).Name;
        }
    }
}
