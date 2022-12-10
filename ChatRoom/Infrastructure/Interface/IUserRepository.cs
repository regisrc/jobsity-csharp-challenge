using Infrastructure.Entity;

namespace Infrastructure.Interface
{
    public interface IUserRepository : IRepositoryAsync<UserEntity>
    {
        Task<UserEntity?> GetByCredentials(string login, string password);

        Task<UserEntity?> GetByLogin(string login);

        Task<UserEntity?> GetByToken(Guid token);
    }
}
