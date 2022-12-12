using Application.Dto;
using Application.Event;
using Application.Service;
using Infrastructure.Entity;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.Application
{
    public class UserServiceTest
    {
        private readonly Mock<ILogger<UserService>> _logger;
        private readonly Mock<IUserEventPublisher> _publisher;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _logger = new Mock<ILogger<UserService>>();
            _publisher = new Mock<IUserEventPublisher>();
            _userRepository = new Mock<IUserRepository>();
            _userService = new UserService(
                _logger.Object,
                _userRepository.Object,
                _publisher.Object);
        }

        [Fact]
        public async Task CreateUser_User_Should_Already_Been_Created()
        {
            var dto = new UserCreateDto
            {
                Name = "Test",
                Login = "Test",
                Password = "Test"
            };

            _userRepository.Setup(x => x.GetByLogin(It.IsAny<string>())).ReturnsAsync(new UserEntity());

            await _userService.CreateUser(dto);

            _userRepository.Verify(x => x.Add(It.IsAny<UserEntity>()), Times.Never);
        }

        [Fact]
        public async Task CreateUser_User_Should_Create()
        {
            var dto = new UserCreateDto
            {
                Name = "Test",
                Login = "Test",
                Password = "Test"
            };

            _userRepository.Setup(x => x.GetByLogin(It.IsAny<string>())).ReturnsAsync(null as UserEntity);

            var salt = "$2a$06$DCq7YPn5Rq63x1Lad4cll.";
            Environment.SetEnvironmentVariable("bcrypt_salt", salt);

            await _userService.CreateUser(dto);

            _userRepository.Verify(x => x.Add(It.IsAny<UserEntity>()), Times.Once);

            _userRepository.Verify(x => x.Add(It.Is<UserEntity>(
                x => x.Name == dto.Name
                && x.Login == BCrypt.Net.BCrypt.HashPassword(dto.Login, salt)
                && x.Password == BCrypt.Net.BCrypt.HashPassword(dto.Password, salt))), Times.Once);
        }

        [Fact]
        public async Task Login_Should_Not_Be_Correct()
        {
            var salt = "$2a$06$DCq7YPn5Rq63x1Lad4cll.";

            var dto = new UserDto
            {
                Login = "Test",
                Password = "Test",
            };

            _userRepository.Setup(x => x.GetByCredentials(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(null as UserEntity);

            Environment.SetEnvironmentVariable("bcrypt_salt", salt);

            var result = await _userService.Login(dto);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Null(result.Token);
            Assert.Null(result.UserId);

            _userRepository.Verify(x => x.GetByCredentials(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _userRepository.Verify(x => x.GetByCredentials(BCrypt.Net.BCrypt.HashPassword(dto.Login, salt), BCrypt.Net.BCrypt.HashPassword(dto.Login, salt)), Times.Once);

            _userRepository.Verify(x => x.Update(It.IsAny<UserEntity>()), Times.Never);
        }

        [Fact]
        public async Task Login_Should_Be_Correct()
        {
            var salt = "$2a$06$DCq7YPn5Rq63x1Lad4cll.";

            var dto = new UserDto
            {
                Login = "Test",
                Password = "Test",
            };

            var entity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Login = BCrypt.Net.BCrypt.HashPassword(dto.Login, salt),
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password, salt),
            };

            _userRepository.Setup(x => x.GetByCredentials(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(entity);

            Environment.SetEnvironmentVariable("bcrypt_salt", salt);

            var result = await _userService.Login(dto);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Token);
            Assert.NotNull(result.UserId);

            _userRepository.Verify(x => x.GetByCredentials(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _userRepository.Verify(x => x.GetByCredentials(BCrypt.Net.BCrypt.HashPassword(dto.Login, salt), BCrypt.Net.BCrypt.HashPassword(dto.Login, salt)), Times.Once);

            _userRepository.Verify(x => x.Update(It.IsAny<UserEntity>()), Times.Once);
            _userRepository.Verify(x => x.Update(It.Is<UserEntity>(
                x => x.Id == entity.Id
                && x.Name == entity.Name
                && x.Login == entity.Login
                && x.Password == entity.Password)), Times.Once);
        }

        [Fact]
        public async Task Logoff_Should_Not_Find_User()
        {
            var logoffEvent = new UserLogoffEvent
            {
                Id = Guid.NewGuid(),
            };

            _userRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(null as UserEntity);

            await _userService.Logoff(logoffEvent);

            _userRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.GetById(It.Is<Guid>(x => x == logoffEvent.Id)), Times.Once);

            _userRepository.Verify(x => x.Update(It.IsAny<UserEntity>()), Times.Never);
        }

        [Fact]
        public async Task Logoff_Should_Logoff()
        {
            var logoffEvent = new UserLogoffEvent
            {
                Id = Guid.NewGuid(),
            };

            var salt = "$2a$06$DCq7YPn5Rq63x1Lad4cll.";

            var entity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Login = BCrypt.Net.BCrypt.HashPassword("Test", salt),
                Password = BCrypt.Net.BCrypt.HashPassword("Test", salt),
                LoggedToken = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                TokenExpirationDate = DateTime.UtcNow.AddMinutes(30),
                UpdateDate = DateTime.UtcNow
            };

            _userRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(entity);

            await _userService.Logoff(logoffEvent);

            _userRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.GetById(It.Is<Guid>(x => x == logoffEvent.Id)), Times.Once);

            _userRepository.Verify(x => x.Update(It.IsAny<UserEntity>()), Times.Once);
            _userRepository.Verify(x => x.Update(It.Is<UserEntity>(
                x => x.Id == entity.Id
                && x.Name == entity.Name
                && x.CreationDate == entity.CreationDate
                && x.Login == entity.Login
                && x.TokenExpirationDate == null
                && x.LoggedToken == null)), Times.Once);
        }

        [Fact]
        public async Task VerifyToken_Should_Have_Token_Null()
        {
            var token = Guid.NewGuid();

            var result = await _userService.VerifyToken(null);

            Assert.False(result);

            _publisher.Verify(x => x.Publish(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public async Task VerifyToken_Should_Have_TokenExpirationDate_null()
        {
            var token = Guid.NewGuid();

            var salt = "$2a$06$DCq7YPn5Rq63x1Lad4cll.";

            var entity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Login = BCrypt.Net.BCrypt.HashPassword("Test", salt),
                Password = BCrypt.Net.BCrypt.HashPassword("Test", salt),
                LoggedToken = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                TokenExpirationDate = null,
                UpdateDate = DateTime.UtcNow
            };

            _userRepository.Setup(x => x.GetByToken(It.IsAny<Guid>())).ReturnsAsync(entity);

            var result = await _userService.VerifyToken(token);

            Assert.False(result);

            _publisher.Verify(x => x.Publish(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public async Task VerifyToken_Should_Have_TokenExpirationDate_Expired()
        {
            var token = Guid.NewGuid();

            var salt = "$2a$06$DCq7YPn5Rq63x1Lad4cll.";

            var entity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Login = BCrypt.Net.BCrypt.HashPassword("Test", salt),
                Password = BCrypt.Net.BCrypt.HashPassword("Test", salt),
                LoggedToken = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                TokenExpirationDate = DateTime.UtcNow.AddMinutes(-30),
                UpdateDate = DateTime.UtcNow
            };

            _userRepository.Setup(x => x.GetByToken(It.IsAny<Guid>())).ReturnsAsync(entity);

            var result = await _userService.VerifyToken(token);

            Assert.False(result);

            _publisher.Verify(x => x.Publish(It.IsAny<UserLogoffEvent>()), Times.Once);

            _publisher.Verify(x => x.Publish(It.Is<UserLogoffEvent>(x => x.Id == entity.Id)), Times.Once);
        }

        [Fact]
        public async Task VerifyToken_Should_Validate()
        {
            var token = Guid.NewGuid();

            var salt = "$2a$06$DCq7YPn5Rq63x1Lad4cll.";

            var entity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Login = BCrypt.Net.BCrypt.HashPassword("Test", salt),
                Password = BCrypt.Net.BCrypt.HashPassword("Test", salt),
                LoggedToken = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                TokenExpirationDate = DateTime.UtcNow.AddMinutes(30),
                UpdateDate = DateTime.UtcNow
            };

            _userRepository.Setup(x => x.GetByToken(It.IsAny<Guid>())).ReturnsAsync(entity);

            var result = await _userService.VerifyToken(token);

            Assert.True(result);

            _publisher.Verify(x => x.Publish(It.IsAny<object>()), Times.Never);
        }
    }
}
