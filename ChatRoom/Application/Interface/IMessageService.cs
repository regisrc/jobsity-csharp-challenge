using Application.Event;

namespace Application.Interface
{
    public interface IMessageService
    {
        void SaveMessage(MessageEvent messageEvent);
    }
}
