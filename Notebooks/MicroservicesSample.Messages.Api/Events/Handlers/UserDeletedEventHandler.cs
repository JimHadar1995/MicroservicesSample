using System;
using System.Linq;
using System.Threading.Tasks;
using MicroservicesSample.Common.EventBus;
using MicroservicesSample.Notebooks.Api.Entities;
using MicroservicesSample.Notebooks.Api.Repositories;

namespace MicroservicesSample.Notebooks.Api.Events.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDeletedEventHandler : IEventBusIntegrationEvent<UserDeletedEvent>
    {
        private readonly NotebookDbContext _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public UserDeletedEventHandler(NotebookDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        /// <inheritdoc />
        public async Task Handle(UserDeletedEvent @event)
        {
            try
            {
                var notes = _dbContext.Set<Notebook>()
                    .Where(_ => _.SenderId == @event.UserId);
                _dbContext.RemoveRange(notes);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
