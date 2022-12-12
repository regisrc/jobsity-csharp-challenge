using Application.Dto;
using Application.Event;
using Application.Interface;
using Infrastructure.Entity;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;

namespace Application.Service
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserEventPublisher _publisher;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository, IUserEventPublisher publisher)
        {
            _logger = logger;
            _userRepository = userRepository;
            _publisher = publisher;
        }

        public async Task CreateUser(UserCreateDto userDto)
        {
            var result = await _userRepository.GetByLogin(userDto.Login);

            if (result != null)
            {
                _logger.LogInformation("User already created");

                return;
            }

            var salt = Environment.GetEnvironmentVariable("bcrypt_salt");

            var entity = new UserEntity
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Login = BCrypt.Net.BCrypt.HashPassword(userDto.Login, salt),
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password, salt),
                Name = userDto.Name
            };

            await _userRepository.Add(entity);

            _logger.LogInformation("User Saved");
        }

        public async Task<LoginResponseDto> Login(UserDto userDto)
        {
            var salt = Environment.GetEnvironmentVariable("bcrypt_salt");

            var result = await _userRepository.GetByCredentials(
                BCrypt.Net.BCrypt.HashPassword(userDto.Login, salt),
                BCrypt.Net.BCrypt.HashPassword(userDto.Password, salt));

            if (result == null)
            {
                _logger.LogInformation("The credentials were incorrect");

                return new LoginResponseDto { Success = false, Token = null, UserId = null };
            }

            var token = Guid.NewGuid();
            result.LoggedToken = token;
            result.UpdateDate = DateTime.UtcNow;
            result.TokenExpirationDate = DateTime.UtcNow.AddMinutes(30);

            await _userRepository.Update(result);

            return new LoginResponseDto { Success = true, Token = token, UserId = result.Id };
        }

        public async Task Logoff(UserLogoffEvent userLogoffEvent)
        {
            var result = await _userRepository.GetById(userLogoffEvent.Id);

            if (result == null)
            {
                _logger.LogInformation("User not found");

                return;
            }

            result.LoggedToken = null;
            result.UpdateDate = DateTime.UtcNow;
            result.TokenExpirationDate = null;

            await _userRepository.Update(result);
        }

        public async Task<bool> VerifyToken(Guid? token)
        {
            if (token == null)
            {
                _logger.LogInformation("Token is null");

                return false;
            }

            var result = await _userRepository.GetByToken(token.Value);

            if (result == null)
            {
                _logger.LogInformation("Token not found");

                return false;
            }

            if (!result.TokenExpirationDate.HasValue)
            {
                _logger.LogInformation("TokenExpirationDate value not found");

                return false;
            }

            if (DateTime.Compare(DateTime.UtcNow, result.TokenExpirationDate.Value) > 0)
            {
                _logger.LogInformation("Token was expired, login again");

                var @event = new UserLogoffEvent { Id = result.Id };

                _publisher.Publish(@event);

                return false;
            }

            return true;
        }
    }
}
