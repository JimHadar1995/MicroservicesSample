using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroservicesSample.Common.Database;
using MicroservicesSample.Identity.Infrastructure.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesSample.Identity.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentitySampleDbContext _dbContext;
        private Dictionary<Type, object> Repositories { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public UnitOfWork(IdentitySampleDbContext dbContext)
        {
            _dbContext = dbContext;
            Repositories = new Dictionary<Type, object>();
        }

        /// <inheritdoc />
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (Repositories.TryGetValue(type, out var value))
            {
                return value as IRepository<T> ?? throw new InvalidOperationException();
            }

            IRepository<T> repo = new Repository<T>(_dbContext);
            Repositories.Add(type, repo);
            return repo;
        }

        /// <inheritdoc />
        public Task<int> Commit() => _dbContext.SaveChangesAsync();

        /// <inheritdoc />
        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        /// <inheritdoc />
        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        /// <inheritdoc />
        public void RollbackTransaction()
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                _dbContext.Database.RollbackTransaction();
            }
        }
    }
}
