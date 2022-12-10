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

        public UserService(ILogger<MessageService> logger, IUserRepository userRepository, IUserEventPublisher publisher)
        {
            _logger = logger;
            _userRepository = userRepository;
            _publisher = publisher;
        }

        public async Task CreateUser(UserDto userDto)
        {
            var result = await _userRepository.GetByLogin(userDto.Login);

            if (result == null)
            {
                _logger.LogInformation("User already created");

                return;
            }

            var entity = new UserEntity
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Login = userDto.Login,
                Password = userDto.Password
            };

            await _userRepository.Add(entity);

            _logger.LogInformation("User Saved");
        }

        public async Task<LoginResponseDto> Login(UserDto userDto)
        {
            var result = await _userRepository.GetByCredentials(userDto.Login, userDto.Password);

            if (result == null)
            {
                _logger.LogInformation("The credentials were incorrect");

                return new LoginResponseDto { Success = false, Token = null };
            }

            var token = Guid.NewGuid();
            result.LoggedToken = token;
            result.UpdateDate = DateTime.UtcNow;
            result.TokenExpirationDate = DateTime.UtcNow.AddMinutes(30);

            await _userRepository.Update(result);

            return new LoginResponseDto { Success = true, Token = token };
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
