using CustomerCRUD.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerCRUD.API
{

    public class CustomerCRUDDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomerCRUDDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}