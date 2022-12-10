using Infrastructure.Interface;

namespace Infrastructure.MessageBroker
{
    public class ChatRoomEventPublisher : Publisher, IChatRoomEventPublisher
    {
        public void Publish(object message)
        {
            Publish(message, Environment.GetEnvironmentVariable("message_broker_chatroom_queue") ?? string.Empty);
        }
    }
}