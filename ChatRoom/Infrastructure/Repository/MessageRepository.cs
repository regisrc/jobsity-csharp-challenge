using Infrastructure.Entity;
using Infrastructure.Interface;

namespace Infrastructure.Repository
{
    public class MessageRepository : RepositoryAsync<MessageEntity>, IMessageRepository
    {
        public MessageRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
