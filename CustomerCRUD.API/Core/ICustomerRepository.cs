using CustomerCRUD.API.Entities;
using CustomerCRUD.API.Models;
namespace CustomerCRUD.API.Core
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        //this interface can  be used if we have any Customer related functionality to implement. otherwise we can stick to methods in Igeneric

    }
}
