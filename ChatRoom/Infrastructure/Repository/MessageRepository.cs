using Infrastructure.Context;
using Infrastructure.Entity;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repository
{
    public class MessageRepository : RepositoryAsync<MessageEntity>, IMessageRepository
    {
        public MessageRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<List<MessageEntity>> GetByChatRoomId(Guid chatRoomId)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<ChatRoomContext>();
            return context.Set<MessageEntity>()
                .Where(x => x.ChatRoomId == chatRoomId)
                .OrderBy(x => x.CreationDate)
                .AsEnumerable()
                .TakeLast(50)
                .ToList();
        }
    }
}
