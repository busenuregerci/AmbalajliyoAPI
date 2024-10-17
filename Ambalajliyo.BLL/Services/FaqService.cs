using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Ambalajliyo.DAL.AbstractRepository;
using Ambalajliyo.DAL.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Services
{
    /// <summary>
    /// Provides implementation for FAQ-related operations defined in the <see cref="IFaqService"/> interface.
    /// </summary>
    public class FaqService : IFaqService
    {
        private readonly IRepository<Faq> _faqRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaqService"/> class.
        /// </summary>
        /// <param name="faqRepository">The repository to handle FAQ data access.</param>
        public FaqService(IRepository<Faq> faqRepository)
        {
            _faqRepository = faqRepository;
        }

        /// <summary>
        /// Creates a new FAQ entry.
        /// </summary>
        /// <param name="faqDto">The FAQ data to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="FaqDto"/> object.</returns>
        public async Task<FaqDto> CreateFaqAsync(FaqDto faqDto)
        {
            var faq = faqDto.Adapt<Faq>(); // Map DTO to entity
            await _faqRepository.AddAsync(faq);
            // Map entity back to DTO
            return faq.Adapt<FaqDto>();
        }

        /// <summary>
        /// Deletes an FAQ entry by its identifier.
        /// </summary>
        /// <param name="faqId">The unique identifier of the FAQ to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteFaqAsync(Guid faqId)
        {
            await _faqRepository.DeleteAsync(faqId);
        }

        /// <summary>
        /// Retrieves all FAQ entries.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="FaqDto"/> objects.</returns>
        public async Task<IEnumerable<FaqDto>> GetAllFaqAsync()
        {
            var faqs = await _faqRepository.GetAllAsync();
            // Map list of entities to list of DTOs
            return faqs.Adapt<List<FaqDto>>();
        }

        /// <summary>
        /// Retrieves a specific FAQ entry by its identifier.
        /// </summary>
        /// <param name="faqId">The unique identifier of the FAQ.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="FaqDto"/> object with the specified identifier.</returns>
        public async Task<FaqDto> GetFaqByIdAsync(Guid faqId)
        {
            var faq = await _faqRepository.GetByIdAsync(faqId);
            // Map entity to DTO
            return faq.Adapt<FaqDto>();
        }

        /// <summary>
        /// Updates an existing FAQ entry.
        /// </summary>
        /// <param name="faqId">The unique identifier of the FAQ to be updated.</param>
        /// <param name="faqDto">The updated FAQ data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateFaqAsync(Guid faqId, FaqDto faqDto)
        {
            var faq = await _faqRepository.GetByIdAsync(faqId);

            // Update FAQ properties
            faq.Id = faqDto.Id;
            faq.Question = faqDto.Question;
            faq.Answer = faqDto.Answer;

            await _faqRepository.UpdateAsync(faq);
        }
    }
}
