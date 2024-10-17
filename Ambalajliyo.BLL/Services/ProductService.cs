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
    /// Provides implementation for product-related operations defined in the <see cref="IProductService"/> interface.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepository">The repository to handle product data access.</param>
        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDto">The product data to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="ProductDto"/> object.</returns>
        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            var product = productDto.Adapt<Product>(); // Map DTO to entity
            await _productRepository.AddAsync(product);
            // Map entity back to DTO
            return product.Adapt<ProductDto>();
        }

        /// <summary>
        /// Deletes a product by its identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteProduct(Guid productId)
        {
            await _productRepository.DeleteAsync(productId);
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="ProductDto"/> objects.</returns>
        public async Task<List<ProductDto>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            // Map list of entities to list of DTOs
            return products.Adapt<List<ProductDto>>();
        }

        /// <summary>
        /// Retrieves a specific product by its identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ProductDto"/> object with the specified identifier.</returns>
        public async Task<ProductDto> GetProductById(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            // Map entity to DTO
            return product.Adapt<ProductDto>();
        }

        /// <summary>
        /// Retrieves products with optional included properties.
        /// </summary>
        /// <param name="includes">An array of strings specifying the related properties to include in the results.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="ProductDto"/> objects.</returns>
        public async Task<List<ProductDto>> GetProductsWithIncludes(string[] includes)
        {
            // Retrieve products with or without includes based on the parameter
            var products = includes == null || includes.Length == 0
                ? await _productRepository.GetAllAsync()
                : await _productRepository.GetAllWithIncludes(includes);

            // Map list of entities to list of DTOs
            return products.Adapt<List<ProductDto>>();
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to be updated.</param>
        /// <param name="productDto">The updated product data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateProduct(Guid productId, ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            // Update product properties
            product.Id = productDto.Id;
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Image = productDto.Image;
            product.CategoryId = productDto.CategoryId;

            await _productRepository.UpdateAsync(product);
        }
    }
}
