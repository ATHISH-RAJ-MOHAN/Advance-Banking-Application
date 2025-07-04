using AccountsApiService.Infrastructure;
using AccountsApiService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountsApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActiveCustomers()
        {
            try
            {
                var customers = await _customerRepository.GetAllActiveCustomersAsync();
                if (customers == null || customers.Count == 0)
                {
                    return BadRequest(new { Message = "No active customers found." });
                }
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An internal server error has occurred: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActiveCustomerById(int id)
        {
            try
            {
                var customer = await _customerRepository.GetActiveCustomerByIdAsync(id);
                if (customer == null)
                {
                    return BadRequest(new { Message = $"No active customer found with ID {id}." });
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An internal server error has occurred: {ex.Message}" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] Customer customer)
        {
            // Check for null customer object
            if (customer == null)
            {
                return BadRequest(new { Error = "Customer data cannot be null." });
            }

            // Validate model state - now simplified due to centralized handling
            if (!ModelState.IsValid)
            {
                // Directly return BadRequest; specific errors are formatted in Program.cs
                return BadRequest(ModelState);
            }

            // Ensure the customer ID is valid
            if (customer.CustomerId <= 0)
            {
                return BadRequest(new { Error = "Invalid Customer ID." });
            }

            try
            {
                var updatedCustomer = await _customerRepository.UpdateCustomerAsync(customer);
                if (updatedCustomer == null)
                {
                    return NotFound(new { Error = $"No active customer found with ID {customer.CustomerId}." });
                }

                return Ok(new { Message = "Customer updated successfully.", Customer = updatedCustomer });
            }
            catch (Exception ex)
            {
                // Handle exceptions internally
                return StatusCode(500, new { Error = $"An internal error occurred: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateCustomer(int id)
        {
            try
            {
                var customer = await _customerRepository.DeactivateCustomerAsync(id);
                if (customer == null)
                {
                    var errorResponse = new Dictionary<string, string>
                {
                    {"Message", "No active customer found or customer could not be deactivated."}
                };
                    return BadRequest(errorResponse); // Status code 400
                }

                var successResponse = new Dictionary<string, object>
            {
                {"Message", $"Customer and all related accounts with CustomerID {customer.CustomerId } deactivated successfully."}
               // {"CustomerID", customer.CustomerId}
            };
                return Ok(successResponse); // Status code 200
            }
            catch (Exception ex)
            {
                var exceptionResponse = new Dictionary<string, string>
            {
                {"Error", $"An error occurred: {ex.Message}"}
            };
                return StatusCode(500, exceptionResponse); // Status code 500
            }
        }

        [HttpGet("userid/{userId}")]
        public async Task<IActionResult> GetCustomerByUserId(int userId)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByUserId(userId);
                if (customer == null)
                {
                    return BadRequest(new { Message = $"No active customer found with userId {userId}." });
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An internal server error has occurred: {ex.Message}" });
            }
        }

        [HttpPut("change")]
        public IActionResult ChangePassword(PasswordChange model)
        {
            try
            {
                _customerRepository.PasswordChange(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
