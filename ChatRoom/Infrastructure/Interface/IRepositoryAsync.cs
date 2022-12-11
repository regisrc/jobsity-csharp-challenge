namespace Infrastructure.Interface
{
    public interface IRepositoryAsync<T>
    {
        Task Add(T entity);

        Task<List<T>> GetAll();

        Task<T?> GetById(Guid id);

        Task Update(T entity);
    }
}
