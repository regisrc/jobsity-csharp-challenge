using Application.Dto;
using Application.Event;

namespace Application.Interface
{
    public interface IUserService
    {
        Task CreateUser(UserDto userDto);

        Task<LoginResponseDto> Login(UserDto userDto);

        Task Logoff(UserLogoffEvent userLogoffEvent);

        Task<bool> VerifyToken(Guid? token);
    }
}
