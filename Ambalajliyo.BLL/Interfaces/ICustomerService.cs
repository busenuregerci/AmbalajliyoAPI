using Ambalajliyo.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Interfaces
{
    /// <summary>
    /// Defines the contract for customer-related operations in the business logic layer.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable list of <see cref="CustomerDto"/> objects.</returns>
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();

        /// <summary>
        /// Retrieves a customer by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CustomerDto"/> object with the specified identifier.</returns>
        Task<CustomerDto> GetCustomerByIdAsync(Guid id);

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customerDto">The customer data to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="CustomerDto"/> object.</returns>
        Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto);

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer to be updated.</param>
        /// <param name="customerDto">The updated customer data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateCustomerAsync(Guid customerId, CustomerDto customerDto);

        /// <summary>
        /// Deletes a customer by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteCustomerAsync(Guid id);
    }
}
