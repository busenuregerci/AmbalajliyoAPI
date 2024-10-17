using Ambalajliyo.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Interfaces
{
    /// <summary>
    /// Defines the contract for FAQ-related operations in the business logic layer.
    /// </summary>
    public interface IFaqService
    {
        /// <summary>
        /// Retrieves a list of all FAQ items.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable list of <see cref="FaqDto"/> objects.</returns>
        Task<IEnumerable<FaqDto>> GetAllFaqAsync();

        /// <summary>
        /// Retrieves a specific FAQ item by its identifier.
        /// </summary>
        /// <param name="faqId">The unique identifier of the FAQ item.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="FaqDto"/> object with the specified identifier.</returns>
        Task<FaqDto> GetFaqByIdAsync(Guid faqId);

        /// <summary>
        /// Creates a new FAQ item.
        /// </summary>
        /// <param name="faqDto">The FAQ data to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="FaqDto"/> object.</returns>
        Task<FaqDto> CreateFaqAsync(FaqDto faqDto);

        /// <summary>
        /// Updates an existing FAQ item.
        /// </summary>
        /// <param name="faqId">The unique identifier of the FAQ item to be updated.</param>
        /// <param name="faqDto">The updated FAQ data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateFaqAsync(Guid faqId, FaqDto faqDto);

        /// <summary>
        /// Deletes a specific FAQ item by its identifier.
        /// </summary>
        /// <param name="faqId">The unique identifier of the FAQ item to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteFaqAsync(Guid faqId);
    }
}
