using Microsoft.EntityFrameworkCore;

namespace CustomerCRUD.API.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected CustomerCRUDDbContext _context;
        internal DbSet<T> _dbset;

            protected ILogger _logger { get; set; }
        public GenericRepository(CustomerCRUDDbContext context,ILogger logger)
        {
            _context = context;
            _logger= logger; 
            _dbset = _context.Set<T>();
        }


       public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
          return await _dbset.AsNoTracking().ToListAsync(); 
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
           _dbset.Remove(entity);
            return true;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
             _dbset.Update(entity);
            return true;
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            await this._dbset.AddAsync(entity);
            return true;
        }


    }
}
