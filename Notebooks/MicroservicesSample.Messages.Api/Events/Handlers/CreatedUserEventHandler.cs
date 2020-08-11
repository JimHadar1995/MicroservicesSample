using System.Threading.Tasks;
using MicroservicesSample.Common.EventBus;
using MicroservicesSample.Notebooks.Api.Events;

namespace MicroservicesSample.Messages.Api.Events.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class CreatedUserEventHandler : IEventBusIntegrationEvent<CreatedUserEvent>
    {
        /// <inheritdoc />
        public Task Handle(CreatedUserEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
