using Infrastructure.Interface;

namespace Infrastructure.MessageBroker
{
    public class MessageEventPublisher : Publisher, IMessageEventPublisher
    {
        public void Publish(object message)
        {
            Publish(message, Environment.GetEnvironmentVariable("message_broker_message_queue") ?? string.Empty);
        }
    }
}