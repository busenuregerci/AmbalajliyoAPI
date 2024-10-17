using Ambalajliyo.DAL.AbstractRepository;
using Ambalajliyo.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambalajliyo.DAL.ConcreteRepository
{
    /// <summary>
    /// Provides the implementation of the repository pattern for CRUD operations.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AmbalajliyoDbContext _context;
        private readonly AmbalajliyoLogDbContext _logDbContext;
        private readonly DbSet<T> _entities;
        private readonly DbSet<T> _logentities;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The database context for entity operations.</param>
        /// <param name="logDbContext">The database context for log operations.</param>
        public Repository(AmbalajliyoDbContext context, AmbalajliyoLogDbContext logDbContext)
        {
            _context = context;
            _logDbContext = logDbContext;
            _entities = _context.Set<T>();
            _logentities = _logDbContext.Set<T>();
        }

        /// <summary>
        /// Asynchronously retrieves all entities.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves all log entries within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A collection of log entries.</returns>
        public async Task<IEnumerable<T>> GetAllLogAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _logentities.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(x => EF.Property<DateTime>(x, "TimeStamp") >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(x => EF.Property<DateTime>(x, "TimeStamp") <= endDate.Value);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The entity with the specified identifier.</returns>
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        /// <summary>
        /// Asynchronously adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>The identifier of the added entity.</returns>
        public async Task<Guid> AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            var idProperty = entity.GetType().GetProperty("Id");
            return (Guid)idProperty.GetValue(entity);
        }

        /// <summary>
        /// Asynchronously updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        public async Task UpdateAsync(T entity)
        {
            _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to be deleted.</param>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously retrieves all entities with the specified related entities included.
        /// </summary>
        /// <param name="includes">The names of the related entities to be included.</param>
        /// <returns>A collection of entities with the specified related entities included.</returns>
        public async Task<IEnumerable<T>> GetAllWithIncludes(params string[] includes)
        {
            IQueryable<T> query = _entities;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
    }
}
