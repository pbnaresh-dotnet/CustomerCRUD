using Microsoft.AspNetCore.Mvc;
using CustomerCRUD.API.Entities;
using CustomerCRUD.API.Models;
using CustomerCRUD.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomerCRUD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerCRUDDbContext dbContext;

        //Can use repository pattern if required for better maintenance and reusability, and also for TDD Approach
        // private readonly ICustomerRepository customerRepository;

        public CustomerController(CustomerCRUDDbContext dbContext)
        {

            this.dbContext = dbContext;
        }

        [HttpGet(Name ="GetCustomers")]
              
        public async Task<ActionResult> GetCustomers()
        {
            try
            {
                var CustomerEntities = await dbContext.Customers.ToListAsync();

                var customers = from custEntity in CustomerEntities
                                select new CustomerModel
                                {
                                    CustomerID = custEntity.CustromerID,
                                    Name = custEntity.Name,
                                    Email = custEntity.Email,
                                    Mobile = custEntity.Mobile,
                                    IsActive = custEntity.IsActive
                                };

                return Ok(customers);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 "Error retrieving data from the database");
            }
        }
        [HttpGet("{id:int}",Name ="GetCustomer")]
        
        public async Task<ActionResult<CustomerModel>> GetCustomer(int id)
        {
            try
            {
                var customerModel = await dbContext.Customers
                    .Select(custEntity => new CustomerModel
                    {
                        CustomerID = custEntity.CustromerID,
                        Name = custEntity.Name,
                        Email = custEntity.Email,
                        Mobile = custEntity.Mobile,
                        IsActive = custEntity.IsActive
                    }).FirstOrDefaultAsync(cust => cust.CustomerID == id);

                if (customerModel == null) return NotFound();


                return customerModel;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        [HttpPost(Name ="CreateCustomer")]
        public async Task<ActionResult<CustomerModel>> CreateCustomer(CustomerModel customerModel)
        {
            try
            {
                if (customerModel == null)
                    return BadRequest();
                //convert customer model to entity

                var newCustomer = new Customer { Email = customerModel.Email, Mobile = customerModel.Email, Name = customerModel.Name, IsActive = true };
                await dbContext.Customers.AddAsync(newCustomer);
                await dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCustomer),
                    new { id = newCustomer.CustromerID }, newCustomer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new customer record");
            }
        }
        [HttpPut(Name = "UpdateCustomer")]
        public async Task<ActionResult> Customer( CustomerModel customerModel)
        {
            var existing = await dbContext.Customers.FindAsync(customerModel.CustomerID);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = customerModel.Name;
            existing.Mobile = customerModel.Mobile;
            existing.Email = customerModel.Email;

            dbContext.ChangeTracker.DetectChanges();
            await dbContext.SaveChangesAsync();

            return Ok(existing);
        }
        [HttpDelete("{id:int}",Name ="DeleteCustomer")]
        public async Task<ActionResult<Customer>> Customer(int id)
        {
            try
            {
                var customerToDelete = await dbContext.Customers.FirstOrDefaultAsync(c => c.CustromerID == id);

                if (customerToDelete == null)
                {
                    return NotFound($"Customer with Id = {id} not found");
                }
                dbContext.Remove(customerToDelete);
                await dbContext.SaveChangesAsync();
                return Ok($"Customer with ID = {id} deleted successfully") ;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

    }
}
