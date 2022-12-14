using Application.Dto;
using Application.Event;

namespace Application.Interface
{
    public interface IChatRoomService
    {
        void CreateChatRoom(ChatRoomCreateDto chatRoomDto);

        Task SaveChatRoom(ChatRoomEvent chatRoomEvent);

        Task<List<ChatRoomDto>> GetChatRooms();
    }
}
