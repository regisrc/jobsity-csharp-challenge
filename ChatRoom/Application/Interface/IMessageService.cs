using Application.Dto;
using Application.Event;

namespace Application.Interface
{
    public interface IMessageService
    {
        void PublishMessage(MessageDto messageDto);

        Task SaveMessage(MessageEvent messageEvent);
    }
}
