using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Messages.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesSample.Messages.Api.Repositories
{
    /// <inheritdoc />
    public class NotebookRepository : INotebookRepository
    {
        private readonly NotebookDbContext _dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public NotebookRepository(NotebookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public IQueryable<Notebook> Query
            => _dbContext.Set<Notebook>();

        /// <inheritdoc />
        public async Task<Notebook> CreateAsync(Notebook message, CancellationToken token)
        {
            _dbContext.Set<Notebook>().Add(message);
            await _dbContext.SaveChangesAsync(token);
            return message;
        }

        /// <inheritdoc />
        public async Task<int> DeleteAsync(string id, CancellationToken token)
        {
            var message = Query.FirstOrDefault(_ => _.Id == id);
            if (message == null)
                return 0;
            _dbContext.Set<Notebook>().Remove(message);
            return await _dbContext.SaveChangesAsync(token);
        }

        /// <inheritdoc />
        public async Task<int> DeleteAsync(Func<Notebook, bool> predicate, CancellationToken token)
        {
            var messages = Query.Where(predicate);
            if (messages.Any())
            {
                _dbContext.Set<Notebook>().RemoveRange(messages);
                return await _dbContext.SaveChangesAsync(token);
            }
            return 0;
        }

        /// <inheritdoc />
        public async Task<Notebook> GetByIdAsync(string id, CancellationToken token)
        {
            var message = await _dbContext.Set<Notebook>().FirstOrDefaultAsync(_ => _.Id == id, token);
            if (message == null)
                throw new EntityNotFoundException();
            return message;
        }
    }
}
