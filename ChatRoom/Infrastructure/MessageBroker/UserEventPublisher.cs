using Infrastructure.Interface;

namespace Infrastructure.MessageBroker
{
    public class UserEventPublisher : Publisher, IUserEventPublisher
    {
        public void Publish(object message)
        {
            Publish(message, Environment.GetEnvironmentVariable("message_broker_user_logoff_queue") ?? string.Empty);
        }
    }
}