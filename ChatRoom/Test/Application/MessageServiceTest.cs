using Application.Dto;
using Application.Event;
using Application.Service;
using Infrastructure.Entity;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.Application
{
    public class MessageServiceTest
    {
        private readonly Mock<ILogger<MessageService>> _logger;
        private readonly Mock<IMessageEventPublisher> _publisher;
        private readonly Mock<IChatRoomRepository> _chatRoomRepository;
        private readonly Mock<IMessageRepository> _messageRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly MessageService _messageService;

        public MessageServiceTest()
        {
            _logger = new Mock<ILogger<MessageService>>();
            _publisher = new Mock<IMessageEventPublisher>();
            _chatRoomRepository = new Mock<IChatRoomRepository>();
            _messageRepository = new Mock<IMessageRepository>();
            _userRepository = new Mock<IUserRepository>();
            _messageService = new MessageService(
                _logger.Object,
                _publisher.Object,
                _messageRepository.Object,
                _chatRoomRepository.Object,
                _userRepository.Object);
        }

        [Fact]
        public void CreateChatRoom_Should_Publish()
        {
            var dto = new MessageCreateDto
            {
                Message = "Hi guys",
                ChatRoomId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            };

            var dto2 = new MessageCreateDto
            {
                Message = dto.Message,
                ChatRoomId = dto.ChatRoomId,
                UserId = Guid.NewGuid(),
            };

            Environment.SetEnvironmentVariable("bot_guid", dto2.UserId.ToString());

            _messageService.PublishMessage(dto);

            _publisher.Verify(x => x.Publish(It.IsAny<object>()), Times.Once);
            _publisher.Verify(x => x.Publish(It.Is<MessageEvent>(
                x => x.UserId == dto.UserId
                && x.ChatRoomId == dto.ChatRoomId
                && x.Message == dto.Message)), Times.Once);

            _publisher.Verify(x => x.PublishBotMessage(It.IsAny<object>()), Times.Once);
            _publisher.Verify(x => x.PublishBotMessage(It.Is<MessageEvent>(
                x => x.UserId == dto2.UserId
                && x.ChatRoomId == dto2.ChatRoomId
                && x.Message == dto2.Message)), Times.Once);
        }

        [Fact]
        public async Task SaveMessage_Should_Not_Find_ChatRoom()
        {
            var messageEvent = new MessageEvent
            {
                ChatRoomId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Message = "Hi guys",
                Guid = Guid.NewGuid(),
            };

            _chatRoomRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(null as ChatRoomEntity);

            await _messageService.SaveMessage(messageEvent);

            _chatRoomRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _chatRoomRepository.Verify(x => x.GetById(It.Is<Guid>(x => x == messageEvent.ChatRoomId)), Times.Once);

            _userRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never);
            _messageRepository.Verify(x => x.Add(It.IsAny<MessageEntity>()), Times.Never);
        }

        [Fact]
        public async Task SaveMessage_Should_Not_Find_User()
        {
            var messageEvent = new MessageEvent
            {
                ChatRoomId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Message = "Hi guys",
                Guid = Guid.NewGuid(),
            };

            _chatRoomRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new ChatRoomEntity());

            _userRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(null as UserEntity);

            var botGuid = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable("bot_guid", botGuid);

            await _messageService.SaveMessage(messageEvent);

            _chatRoomRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _chatRoomRepository.Verify(x => x.GetById(It.Is<Guid>(x => x == messageEvent.ChatRoomId)), Times.Once);

            _userRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.GetById(It.Is<Guid>(x => x == messageEvent.UserId)), Times.Once);

            _messageRepository.Verify(x => x.Add(It.IsAny<MessageEntity>()), Times.Never);
        }

        [Fact]
        public async Task SaveMessage_Should_Save()
        {
            var messageEvent = new MessageEvent
            {
                ChatRoomId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Message = "Hi guys",
                Guid = Guid.NewGuid(),
            };

            _chatRoomRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new ChatRoomEntity());

            _userRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new UserEntity());

            var botGuid = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable("bot_guid", botGuid);

            await _messageService.SaveMessage(messageEvent);

            _chatRoomRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _chatRoomRepository.Verify(x => x.GetById(It.Is<Guid>(x => x == messageEvent.ChatRoomId)), Times.Once);

            _userRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.GetById(It.Is<Guid>(x => x == messageEvent.UserId)), Times.Once);

            _messageRepository.Verify(x => x.Add(It.IsAny<MessageEntity>()), Times.Once);
            _messageRepository.Verify(x => x.Add(It.Is<MessageEntity>(
                x => x.UserId == messageEvent.UserId
                && x.Message == messageEvent.Message
                && x.Id == messageEvent.Guid
                && x.ChatRoomId == messageEvent.ChatRoomId)), Times.Once);
        }

        [Fact]
        public async Task SaveMessage_Should_Save_With_BotId()
        {
            var botGuid = Guid.NewGuid();

            var messageEvent = new MessageEvent
            {
                ChatRoomId = Guid.NewGuid(),
                UserId = botGuid,
                Message = "Hi guys",
                Guid = Guid.NewGuid(),
            };

            _chatRoomRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new ChatRoomEntity());

            _userRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(null as UserEntity);
            
            Environment.SetEnvironmentVariable("bot_guid", botGuid.ToString());

            await _messageService.SaveMessage(messageEvent);

            _chatRoomRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _chatRoomRepository.Verify(x => x.GetById(It.Is<Guid>(x => x == messageEvent.ChatRoomId)), Times.Once);

            _userRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.GetById(It.Is<Guid>(x => x == messageEvent.UserId)), Times.Once);

            _messageRepository.Verify(x => x.Add(It.IsAny<MessageEntity>()), Times.Once);
            _messageRepository.Verify(x => x.Add(It.Is<MessageEntity>(
                x => x.UserId == messageEvent.UserId
                && x.Message == messageEvent.Message
                && x.Id == messageEvent.Guid
                && x.ChatRoomId == messageEvent.ChatRoomId)), Times.Once);
        }

        [Fact]
        public async Task GetMessages_Should_Return_Empty_List()
        {
            var chatRoomId = Guid.NewGuid();

            _messageRepository.Setup(x => x.GetByChatRoomId(It.IsAny<Guid>())).ReturnsAsync(new List<MessageEntity>());

            var results = await _messageService.GetMessages(chatRoomId);

            Assert.Empty(results);
        }

        [Fact]
        public async Task GetMessages_Should_Return_List()
        {
            var chatRoomId = Guid.NewGuid();

            var entities = new List<MessageEntity>
            {
                new MessageEntity
                {
                    ChatRoomId = chatRoomId,
                    CreationDate = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    Message = "Hi guys 1",
                    UserId = Guid.NewGuid()
                },
                new MessageEntity
                {
                    ChatRoomId = chatRoomId,
                    CreationDate = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    Message = "Hi guys 2",
                    UserId = Guid.NewGuid()
                },
                new MessageEntity
                {
                    ChatRoomId = chatRoomId,
                    CreationDate = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    Message = "Hi guys 3",
                    UserId = Guid.NewGuid()
                },
            };

            var userEntities = new List<UserEntity>();

            entities.ForEach(x => userEntities.Add(new UserEntity { Id = x.UserId, Name = "Teste" }));

            Environment.SetEnvironmentVariable("bot_guid", Guid.NewGuid().ToString());

            _messageRepository.Setup(x => x.GetByChatRoomId(It.IsAny<Guid>())).ReturnsAsync(entities);
            _userRepository.Setup(x => x.GetAll()).ReturnsAsync(userEntities);

            var results = await _messageService.GetMessages(chatRoomId);

            Assert.NotEmpty(results);

            foreach (var item in results)
            {
                var entity = entities.Find(x => x.Message == item.Message);

                Assert.NotNull(entity);
                Assert.Equal(item.Message, entity.Message);
                Assert.Equal(item.UserId, entity.UserId);
                Assert.Equal(item.ChatRoomId, entity.ChatRoomId);
            }
        }

        [Fact]
        public void GetUserName_Should_Get_UserName()
        {
            var entities = new List<UserEntity>
            {
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Test1",
                    Login = "Test1",
                    Password = "Test1"
                },
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Test2",
                    Login = "Test2",
                    Password = "Test2"
                },
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Test3",
                    Login = "Test3",
                    Password = "Test3"
                }
            };

            var botGuid = Guid.NewGuid();

            Environment.SetEnvironmentVariable("bot_guid", botGuid.ToString());

            var result1 = _messageService.GetUserName(entities, botGuid);

            Assert.Equal("Bot", result1);

            var result2 = _messageService.GetUserName(entities, entities.First().Id);

            Assert.Equal(entities.First().Name, result2);
        }
    }
}
