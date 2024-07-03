using CustomerCRUD.API.Core.Repositories;

namespace CustomerCRUD.API.Core
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private CustomerCRUDDbContext _dbContext;
        public ICustomerRepository CustomerRepository{ get; }

        public UnitOfWork(CustomerCRUDDbContext dbContext,ILoggerFactory loggerFactory) { 
         _dbContext = dbContext;
           var  _logger = loggerFactory.CreateLogger("CustomerCRUDlogs");
            CustomerRepository = new CustomerRepository(_dbContext, _logger);
        }

        public void Dispose() { _dbContext.Dispose(); }
     
        public async Task SaveChangesAsync()
        {
           await _dbContext.SaveChangesAsync();
            
        }

    }
}
 