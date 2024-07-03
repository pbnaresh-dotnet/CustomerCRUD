namespace CustomerCRUD.API.Core
{
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        Task SaveChangesAsync();
    }
}
