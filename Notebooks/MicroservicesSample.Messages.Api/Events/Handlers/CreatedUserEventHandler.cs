using System.Threading.Tasks;
using MicroservicesSample.Common.EventBus;

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
