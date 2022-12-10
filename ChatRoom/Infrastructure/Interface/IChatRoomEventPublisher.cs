namespace Infrastructure.Interface
{
    public interface IChatRoomEventPublisher
    {
        void Publish(object message);
    }
}
