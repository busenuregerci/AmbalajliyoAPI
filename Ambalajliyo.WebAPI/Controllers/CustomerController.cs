using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ambalajliyo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Bütün müşterileri getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        /// <summary>
        /// Belirtilen id'ye ait müşteriyi getirir.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("GetCustomerById/{customerId}")]
        public async Task<IActionResult> GetCustomerById(Guid customerId)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        /// <summary>
        /// Yeni bir müşteri ekler.
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            await _customerService.CreateCustomerAsync(customerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { customerId = customerDto.Id }, customerDto);
        }

        /// <summary>
        /// Belirtilen id'ye ait müşteriyi günceller.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateCustomer/{customerId}")]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, [FromBody] CustomerDto customerDto)
        {
            if (customerId != customerDto.Id) return BadRequest();

            await _customerService.UpdateCustomerAsync(customerId, customerDto);
            return NoContent();
        }

        /// <summary>
        /// Belirtilen id'ye ait müşteriyi siler.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCustomer/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            await _customerService.DeleteCustomerAsync(customerId);
            return NoContent();
        }
    }
}