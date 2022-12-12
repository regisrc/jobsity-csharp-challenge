using Application.Dto;
using Application.Event;
using Application.Service;
using Infrastructure.Entity;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.Application
{
    public class ChatRoomServiceTest
    {
        private readonly Mock<ILogger<ChatRoomService>> _logger;
        private readonly Mock<IChatRoomEventPublisher> _publisher;
        private readonly Mock<IChatRoomRepository> _chatRoomRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly ChatRoomService _chatRoomService;

        public ChatRoomServiceTest()
        {
            _logger = new Mock<ILogger<ChatRoomService>>();
            _publisher = new Mock<IChatRoomEventPublisher>();
            _chatRoomRepository = new Mock<IChatRoomRepository>();
            _userRepository = new Mock<IUserRepository>();
            _chatRoomService = new ChatRoomService(_logger.Object, _publisher.Object, _chatRoomRepository.Object, _userRepository.Object);
        }

        [Fact]
        public void CreateChatRoom_Should_Publish()
        {
            var dto = new ChatRoomCreateDto
            {
                CreatorId = Guid.NewGuid(),
                Name = "name"
            };

            _chatRoomService.CreateChatRoom(dto);

            _publisher.Verify(x => x.Publish(It.IsAny<object>()), Times.Once);
            _publisher.Verify(x => x.Publish(It.Is<ChatRoomEvent>(
                x => x.CreatorId == dto.CreatorId
                && x.Name == dto.Name)), Times.Once);
        }

        [Fact]
        public async Task SaveChatRoom_Should_Not_Find_User()
        {
            var @event = new ChatRoomEvent
            {
                CreatorId = Guid.NewGuid(),
                Name = "name",
                Guid = Guid.NewGuid()
            };

            _userRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(null as UserEntity);

            await _chatRoomService.SaveChatRoom(@event);

            _chatRoomRepository.Verify(x => x.Add(It.IsAny<ChatRoomEntity>()), Times.Never);
        }

        [Fact]
        public async Task SaveChatRoom_Should_Add()
        {
            var @event = new ChatRoomEvent
            {
                CreatorId = Guid.NewGuid(),
                Name = "name",
                Guid = Guid.NewGuid()
            };

            var entity = new UserEntity { Id = @event.CreatorId };

            _userRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(entity);

            await _chatRoomService.SaveChatRoom(@event);

            _chatRoomRepository.Verify(x => x.Add(It.IsAny<ChatRoomEntity>()), Times.Once);
            _chatRoomRepository.Verify(x => x.Add(It.Is<ChatRoomEntity>(
                x => x.CreatorId == @event.CreatorId
                && x.Name == @event.Name
                && x.Id == @event.Guid)), Times.Once);
        }

        [Fact]
        public async Task GetChatRooms_Should_Get_Data()
        {
            var entities = new List<ChatRoomEntity>
            {
                new ChatRoomEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "name1",
                    CreatorId = Guid.NewGuid(),
                },
                new ChatRoomEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "name2",
                    CreatorId = Guid.NewGuid(),
                },
                new ChatRoomEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "name3",
                    CreatorId = Guid.NewGuid(),
                }
            };

            _chatRoomRepository.Setup(x => x.GetAll()).ReturnsAsync(entities);

            var results = await _chatRoomService.GetChatRooms();

            foreach (var item in results)
            {
                var entity = entities.Find(x => x.Id == item.Id);

                Assert.NotNull(entity);
                Assert.Equal(item.Name, entity.Name);
                Assert.Equal(item.Id, entity.Id);
                Assert.Equal(item.CreatorId, entity.CreatorId);
            }
        }
    }
}
