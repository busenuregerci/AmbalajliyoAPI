using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Ambalajliyo.DAL.AbstractRepository;
using Ambalajliyo.DAL.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Services
{
    /// <summary>
    /// Provides implementation for customer-related operations defined in the <see cref="ICustomerService"/> interface.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService"/> class.
        /// </summary>
        /// <param name="customers">The repository to handle customer data access.</param>
        public CustomerService(IRepository<Customer> customers)
        {
            _customers = customers;
        }

        /// <summary>
        /// Retrieves all customers, sorted by creation date in descending order.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="CustomerDto"/> objects.</returns>
        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _customers.GetAllAsync();
            var sortedCustomers = customers.OrderByDescending(c => c.CreatedDate).ToList();

            // Map entity list to DTO list
            return sortedCustomers.Adapt<List<CustomerDto>>();
        }

        /// <summary>
        /// Retrieves a specific customer by its identifier.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CustomerDto"/> object with the specified identifier.</returns>
        public async Task<CustomerDto> GetCustomerByIdAsync(Guid customerId)
        {
            var customer = await _customers.GetByIdAsync(customerId);
            // Map entity to DTO
            return customer.Adapt<CustomerDto>();
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customerDto">The customer data to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="CustomerDto"/> object.</returns>
        public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
        {
            var customer = customerDto.Adapt<Customer>(); // Map DTO to entity
            await _customers.AddAsync(customer);
            // Map entity back to DTO
            return customer.Adapt<CustomerDto>();
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer to be updated.</param>
        /// <param name="customerDto">The updated customer data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateCustomerAsync(Guid customerId, CustomerDto customerDto)
        {
            var customer = await _customers.GetByIdAsync(customerId);

            // Update customer properties
            customer.Id = customerDto.Id;
            customer.Name = customerDto.Name;
            customer.Email = customerDto.Email;
            customer.Message = customerDto.Message;
            customer.ProductId = customerDto.ProductId;
            customer.IsItAnswered = customerDto.IsItAnswered;
            customer.ModifiedDate = customerDto.ModifiedDate;

            await _customers.UpdateAsync(customer);
        }

        /// <summary>
        /// Deletes a customer by its identifier.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteCustomerAsync(Guid customerId)
        {
            await _customers.DeleteAsync(customerId);
        }
    }
}
