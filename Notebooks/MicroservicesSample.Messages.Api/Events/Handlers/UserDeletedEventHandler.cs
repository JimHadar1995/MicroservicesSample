using System;
using System.Linq;
using System.Threading.Tasks;
using MicroservicesSample.Common.Database;
using MicroservicesSample.Common.EventBus;
using MicroservicesSample.Messages.Api.Entities;
using MicroservicesSample.Messages.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesSample.Messages.Api.Events.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDeletedEventHandler : IEventBusIntegrationEvent<UserDeletedEvent>
    {
        private readonly NotebookDbContext _dbContext;
        public UserDeletedEventHandler(NotebookDbContext dbContext)
        {
            _dbContext = dbContext;
        }
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
