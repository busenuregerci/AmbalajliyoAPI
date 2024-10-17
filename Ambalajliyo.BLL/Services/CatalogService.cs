using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Ambalajliyo.DAL.AbstractRepository;
using Ambalajliyo.DAL.Entities;
using Mapster;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Services
{
    /// <summary>
    /// Provides implementation for catalog-related operations defined in the <see cref="ICatalogService"/> interface.
    /// </summary>
    public class CatalogService : ICatalogService
    {
        private readonly IRepository<Catalog> _catalogRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogService"/> class.
        /// </summary>
        /// <param name="catalogRepository">The repository to handle catalog data access.</param>
        public CatalogService(IRepository<Catalog> catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        /// <summary>
        /// Creates a new catalog item.
        /// </summary>
        /// <param name="catalogDto">The catalog data to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="CatalogDto"/> object.</returns>
        public async Task<CatalogDto> CreateCatalog(CatalogDto catalogDto)
        {
            try
            {
                var catalog = catalogDto.Adapt<Catalog>(); // Map DTO to entity
                await _catalogRepository.AddAsync(catalog);
                return catalog.Adapt<CatalogDto>(); // Map entity back to DTO
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating the catalog.");
                throw; // Re-throw exception for higher-level handling
            }
        }

        /// <summary>
        /// Deletes a catalog item by its identifier.
        /// </summary>
        /// <param name="catalogId">The unique identifier of the catalog item to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteCatalog(Guid catalogId)
        {
            try
            {
                await _catalogRepository.DeleteAsync(catalogId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the catalog.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a list of all catalog items.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="CatalogDto"/> objects.</returns>
        public async Task<List<CatalogDto>> GetAllCatalog()
        {
            var catalogs = await _catalogRepository.GetAllAsync();
            return catalogs.Adapt<List<CatalogDto>>(); // Map list of entities to list of DTOs
        }

        /// <summary>
        /// Retrieves a specific catalog item by its identifier.
        /// </summary>
        /// <param name="catalogId">The unique identifier of the catalog item.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CatalogDto"/> object with the specified identifier.</returns>
        public async Task<CatalogDto> GetCatalogById(Guid catalogId)
        {
            var catalog = await _catalogRepository.GetByIdAsync(catalogId);
            return catalog.Adapt<CatalogDto>(); // Map entity to DTO
        }

        /// <summary>
        /// Updates an existing catalog item.
        /// </summary>
        /// <param name="catalogId">The unique identifier of the catalog item to be updated.</param>
        /// <param name="catalogDto">The updated catalog data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateCatalog(Guid catalogId, CatalogDto catalogDto)
        {
            var catalog = await _catalogRepository.GetByIdAsync(catalogId);

            // Update catalog properties
            catalog.Id = catalogDto.Id;
            catalog.PdfName = catalogDto.PdfName;

            await _catalogRepository.UpdateAsync(catalog);
        }
    }
}
