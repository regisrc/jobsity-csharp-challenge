using Infrastructure.Context;
using Infrastructure.Entity;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repository
{
    public class UserRepository : RepositoryAsync<UserEntity>, IUserRepository
    {
        public UserRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public virtual async Task<UserEntity?> GetByCredentials(string login, string password)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<ChatRoomContext>();
            return await context.Set<UserEntity>().FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
        }

        public virtual async Task<UserEntity?> GetByLogin(string login)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<ChatRoomContext>();
            return await context.Set<UserEntity>().FirstOrDefaultAsync(x => x.Login == login);
        }

        public virtual async Task<UserEntity?> GetByToken(Guid token)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<ChatRoomContext>();
            return await context.Set<UserEntity>().FirstOrDefaultAsync(x => x.LoggedToken == token);
        }
    }
}
