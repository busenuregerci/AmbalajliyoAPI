using Ambalajliyo.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Interfaces
{
    /// <summary>
    /// Defines the contract for catalog-related operations in the business logic layer.
    /// </summary>
    public interface ICatalogService
    {
        /// <summary>
        /// Retrieves a list of all catalog items.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="CatalogDto"/> objects.</returns>
        Task<List<CatalogDto>> GetAllCatalog();

        /// <summary>
        /// Creates a new catalog item.
        /// </summary>
        /// <param name="catalogDto">The catalog item to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="CatalogDto"/> object.</returns>
        Task<CatalogDto> CreateCatalog(CatalogDto catalogDto);

        /// <summary>
        /// Deletes a catalog item by its identifier.
        /// </summary>
        /// <param name="catalogId">The unique identifier of the catalog item to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteCatalog(Guid catalogId);

        /// <summary>
        /// Updates an existing catalog item.
        /// </summary>
        /// <param name="catalogId">The unique identifier of the catalog item to be updated.</param>
        /// <param name="catalogDto">The updated catalog item data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateCatalog(Guid catalogId, CatalogDto catalogDto);

        /// <summary>
        /// Retrieves a catalog item by its identifier.
        /// </summary>
        /// <param name="catalogId">The unique identifier of the catalog item.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CatalogDto"/> object with the specified identifier.</returns>
        Task<CatalogDto> GetCatalogById(Guid catalogId);
    }
}
