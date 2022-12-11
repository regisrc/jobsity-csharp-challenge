using Application.Event;

namespace Application.Interface
{
    public interface IMessageService
    {
        Task ConsumeMessage(MessageEvent messageEvent);
    }
}
