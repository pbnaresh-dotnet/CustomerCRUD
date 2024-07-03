using CustomerCRUD.API.Entities;
using CustomerCRUD.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerCRUD.API.Core.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
         private CustomerCRUDDbContext _context;
        public CustomerRepository( CustomerCRUDDbContext dbContext,ILogger logger) : base(dbContext, logger)
        {
            _context= dbContext;
        }

        public override async Task<Customer?> GetByIdAsync(int id)
        {
         return   await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c=>c.CustromerID==id);

        }

    }
}
