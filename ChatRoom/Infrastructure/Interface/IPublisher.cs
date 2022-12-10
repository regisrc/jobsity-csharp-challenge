namespace Infrastructure.Interface
{
    public interface IPublisher
    {
        void Publish(object message);
    }
}
