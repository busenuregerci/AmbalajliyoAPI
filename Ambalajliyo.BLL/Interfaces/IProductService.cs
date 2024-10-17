using Ambalajliyo.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Interfaces
{
    /// <summary>
    /// Provides methods for managing products.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of all products.</returns>
        Task<List<ProductDto>> GetAllProducts();

        /// <summary>
        /// Creates a new product and returns its details.
        /// </summary>
        /// <param name="productDto">The data transfer object containing details of the product to create.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the details of the created product.</returns>
        Task<ProductDto> CreateProduct(ProductDto productDto);

        /// <summary>
        /// Deletes a product with the specified ID.
        /// </summary>
        /// <param name="productId">The ID of the product to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteProduct(Guid productId);

        /// <summary>
        /// Updates an existing product with the specified details.
        /// </summary>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="productDto">The data transfer object containing updated details of the product.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateProduct(Guid productId, ProductDto productDto);

        /// <summary>
        /// Retrieves a product with the specified ID.
        /// </summary>
        /// <param name="productId">The ID of the product to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the details of the product.</returns>
        Task<ProductDto> GetProductById(Guid productId);

        /// <summary>
        /// Retrieves all products along with specified related data (Eager Loading).
        /// </summary>
        /// <param name="includes">The names of the related tables to include.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of products with the included related data.</returns>
        Task<List<ProductDto>> GetProductsWithIncludes(string[] includes);
    }
}
