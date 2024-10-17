using Ambalajliyo.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Interfaces
{
    /// <summary>
    /// Provides methods for managing categories.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves all categories asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of all categories.</returns>
        Task<List<CategoryDto>> GetAllCategories();

        /// <summary>
        /// Creates a new category and returns its details.
        /// </summary>
        /// <param name="categoryDto">The data transfer object containing details of the category to create.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the details of the created category.</returns>
        Task<CategoryDto> CreateCategory(CategoryDto categoryDto);

        /// <summary>
        /// Deletes a category with the specified ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteCategory(Guid categoryId);

        /// <summary>
        /// Updates an existing category with the specified details.
        /// </summary>
        /// <param name="categoryId">The ID of the category to update.</param>
        /// <param name="categoryDto">The data transfer object containing updated details of the category.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateCategory(Guid categoryId, CategoryDto categoryDto);

        /// <summary>
        /// Retrieves a category with the specified ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the details of the category.</returns>
        Task<CategoryDto> GetCategoryById(Guid categoryId);

        /// <summary>
        /// Retrieves all categories along with specified related data (Eager Loading).
        /// </summary>
        /// <param name="includes">The names of the related tables to include.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of categories with the included related data.</returns>
        Task<List<CategoryDto>> GetCategoriesWithIncludes(string[] includes);
    }
}
