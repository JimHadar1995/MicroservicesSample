using System;
using System.Linq;
using System.Threading.Tasks;
using MicroservicesSample.Common.EventBus;
using MicroservicesSample.Notebooks.Api.Entities;
using MicroservicesSample.Notebooks.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesSample.Notebooks.Api.Events.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdatedUserEventHandler : IEventBusIntegrationEvent<UpdatedUserEvent>
    {
        private readonly NotebookDbContext _dbContext;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public UpdatedUserEventHandler(NotebookDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <inheritdoc />
        public async Task Handle(UpdatedUserEvent @event)
        {
            try
            {
                var notes = await _dbContext.Set<Notebook>()
                    .Where(_ => _.SenderId == @event.Id)
                    .ToListAsync();
                if (notes.Any())
                {
                    foreach (var notebook in notes)
                    {
                        notebook.SenderId = @event.Id;
                        notebook.SenderName = @event.UserName;
                        _dbContext.Set<Notebook>().Update(notebook);
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
