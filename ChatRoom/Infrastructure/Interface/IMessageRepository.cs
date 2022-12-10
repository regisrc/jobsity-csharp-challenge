using Infrastructure.Entity;

namespace Infrastructure.Interface
{
    public interface IMessageRepository : IRepositoryAsync<MessageEntity>
    { }
}
