using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.DAL.AbstractRepository
{
    /// <summary>
    /// Defines the basic operations for a repository pattern.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Asynchronously retrieves all entities.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves all log entries within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A collection of log entries.</returns>
        Task<IEnumerable<T>> GetAllLogAsync(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Asynchronously retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The entity with the specified identifier.</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Asynchronously adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>The identifier of the added entity.</returns>
        Task<Guid> AddAsync(T entity);

        /// <summary>
        /// Asynchronously updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Asynchronously deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to be deleted.</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Asynchronously retrieves all entities with the specified related entities included.
        /// </summary>
        /// <param name="includes">The names of the related entities to be included.</param>
        /// <returns>A collection of entities with the specified related entities included.</returns>
        Task<IEnumerable<T>> GetAllWithIncludes(params string[] includes);
    }
}
