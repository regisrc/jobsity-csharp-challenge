using Infrastructure.Context;
using Infrastructure.Entity;
using Infrastructure.Interface;

namespace Infrastructure.Repository
{
    public class ChatRoomRepository : RepositoryAsync<ChatRoomEntity>, IChatRoomRepository
    {
        public ChatRoomRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
