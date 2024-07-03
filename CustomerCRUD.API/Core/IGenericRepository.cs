namespace CustomerCRUD.API.Core
{
    public interface IGenericRepository<T> where T : class
    {
    
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<bool> DeleteAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> AddAsync(T entity);

    }
}
