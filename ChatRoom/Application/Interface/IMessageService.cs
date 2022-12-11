using Application.Dto;
using Application.Event;

namespace Application.Interface
{
    public interface IMessageService
    {
        void PublishMessage(MessageCreateDto messageDto);

        Task SaveMessage(MessageEvent messageEvent);

        Task<List<MessageDto>> GetMessages(Guid chatRoomId);
    }
}
