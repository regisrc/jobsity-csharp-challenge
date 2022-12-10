using Application.Dto;

namespace Application.Interface
{
    public interface IChatRoomService
    {
        void CreateChatRoom(ChatRoomDto chatRoomDto);
    }
}
