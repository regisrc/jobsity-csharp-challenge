using Application.Dto;
using Application.Event;

namespace Application.Interface
{
    public interface IMessageService
    {
        void PublishMessage(MessageDto message);

        void SaveMessage(MessageEvent messageEvent);
    }
}
