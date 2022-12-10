namespace Infrastructure.Interface
{
    public interface IUserEventPublisher
    {
        void Publish(object message);
    }
}
