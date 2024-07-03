using Microsoft.AspNetCore.Mvc;
using CustomerCRUD.API.Entities;
using CustomerCRUD.API.Models;
using CustomerCRUD.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;
using CustomerCRUD.API.Core;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomerCRUD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<CustomerController> _logger;

        //Can use repository pattern if required for better maintenance and reusability, and also for TDD Approach
        // private readonly ICustomerRepository customerRepository;

        public CustomerController(IUnitOfWork unitOfWork, ILogger<CustomerController> logger)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet(Name ="GetCustomers")]
              
        public async Task<ActionResult> GetCustomers()
        {
            try
            {
                var customers = await _unitOfWork.CustomerRepository.GetAllAsync();


                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                 "Error retrieving data from the database");
            }
        }
        [HttpGet("{id:int}",Name ="GetCustomer")]
        
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                var customerModel = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
                   

                if (customerModel == null) return NotFound();


                return customerModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        [HttpPost(Name ="CreateCustomer")]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customerModel)
        {
            try
            {
                if (customerModel == null)
                    return BadRequest();
                //convert customer model to entity
               bool isSuccess=await _unitOfWork.CustomerRepository.AddAsync(customerModel);
                if (isSuccess)
                   await _unitOfWork.SaveChangesAsync();

                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new customer record");
            }
        }
        [HttpPut(Name = "UpdateCustomer")]
        public async Task<ActionResult> Customer( Customer customerModel)
        {
            var existing = await _unitOfWork.CustomerRepository.GetByIdAsync(customerModel.CustromerID);

            try
            {
                if (existing == null)
                {
                    _logger.LogInformation("Custoemr Not found");
                    return NotFound();
                }

                await _unitOfWork.CustomerRepository.UpdateAsync(customerModel);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error creating new customer record");
            }
            return Ok(existing);
        }
        [HttpDelete("{id:int}",Name ="DeleteCustomer")]
        public async Task<ActionResult<Customer>> Customer(int id)
        {
            try
            {
                var customerToDelete = await _unitOfWork.CustomerRepository.GetByIdAsync(id);

                if (customerToDelete == null)
                {
                    return NotFound($"Customer with Id = {id} not found");
                }
               await _unitOfWork.CustomerRepository.DeleteAsync(customerToDelete);
                await _unitOfWork.SaveChangesAsync();
                return Ok($"Customer with ID = {id} deleted successfully") ;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

    }
}
