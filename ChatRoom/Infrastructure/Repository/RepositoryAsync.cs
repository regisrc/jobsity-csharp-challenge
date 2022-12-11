using Infrastructure.Context;
using Infrastructure.Entity;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repository
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : BaseEntity
    {
        protected readonly IServiceProvider _serviceProvider;

        public RepositoryAsync(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual async Task Add(T entity)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<ChatRoomContext>();
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public Task<List<T>> GetAll()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<ChatRoomContext>();
            return context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetById(Guid id)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<ChatRoomContext>();
            return await context.Set<T>().FirstOrDefaultAsync(x => Equals(x.Id, id));
        }

        public virtual async Task Update(T entity)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<ChatRoomContext>();
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
