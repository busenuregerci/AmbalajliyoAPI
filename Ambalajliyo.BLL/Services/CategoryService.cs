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
    /// Provides implementation for category-related operations defined in the <see cref="ICategoryService"/> interface.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The repository to handle category data access.</param>
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryDto">The category data to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="CategoryDto"/> object.</returns>
        public async Task<CategoryDto> CreateCategory(CategoryDto categoryDto)
        {
            try
            {
                var category = categoryDto.Adapt<Category>(); // Map DTO to entity
                await _categoryRepository.AddAsync(category);
                return category.Adapt<CategoryDto>(); // Map entity back to DTO
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating the category.");
                throw; // Re-throw exception for higher-level handling
            }
        }

        /// <summary>
        /// Deletes a category by its identifier.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteCategory(Guid categoryId)
        {
            try
            {
                await _categoryRepository.DeleteAsync(categoryId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the category.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a list of all categories.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="CategoryDto"/> objects.</returns>
        public async Task<List<CategoryDto>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Adapt<List<CategoryDto>>(); // Map list of entities to list of DTOs
        }

        /// <summary>
        /// Retrieves a list of categories including related entities.
        /// </summary>
        /// <param name="includes">An array of related entities to include in the result.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="CategoryDto"/> objects.</returns>
        public async Task<List<CategoryDto>> GetCategoriesWithIncludes(string[] includes)
        {
            var categories = await _categoryRepository.GetAllWithIncludes(includes);
            return categories.Adapt<List<CategoryDto>>(); // Map list of entities to list of DTOs
        }

        /// <summary>
        /// Retrieves a specific category by its identifier.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CategoryDto"/> object with the specified identifier.</returns>
        public async Task<CategoryDto> GetCategoryById(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            return category.Adapt<CategoryDto>(); // Map entity to DTO
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category to be updated.</param>
        /// <param name="categoryDto">The updated category data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateCategory(Guid categoryId, CategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            category.Id = categoryDto.Id;
            category.Name = categoryDto.Name;

            await _categoryRepository.UpdateAsync(category);
        }
    }
}
