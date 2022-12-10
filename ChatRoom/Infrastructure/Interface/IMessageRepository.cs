using Infrastructure.Entity;

namespace Infrastructure.Interface
{
    public interface IMessageRepository : IRepositoryAsync<MessageEntity>
    {
        Task<List<MessageEntity>> GetByChatRoomId(Guid chatRoomId);
    }
}
