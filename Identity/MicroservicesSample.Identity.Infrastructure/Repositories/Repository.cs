using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.Common.Database;
using MicroservicesSample.Identity.Infrastructure.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesSample.Identity.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly DbContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(IdentitySampleDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public IQueryable<T> Query => _context.Set<T>();

        /// <inheritdoc />
        public Task<List<T>> GetAsync(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default)
            => Query.Where(match).ToListAsync(cancellationToken);

        /// <inheritdoc />
        public Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
            => Query.ToListAsync(cancellationToken);

        /// <inheritdoc />
        public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default)
            => Query.FirstOrDefaultAsync(match, cancellationToken)!;

        /// <inheritdoc />
        public T Create(T entity)
        {
            return _context.Set<T>().Add(entity).Entity;
        }

        /// <inheritdoc />
        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        /// <inheritdoc />
        public T Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <inheritdoc />
        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        /// <inheritdoc />
        public Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var range = await GetAsync(predicate, cancellationToken).ConfigureAwait(false);
            _context.Set<T>().RemoveRange(range);
            return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
