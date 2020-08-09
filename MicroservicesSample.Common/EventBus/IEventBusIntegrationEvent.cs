using System.Threading.Tasks;

namespace MicroservicesSample.Common.EventBus
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TIntegrationEvent"></typeparam>
    public interface IEventBusIntegrationEvent<in TIntegrationEvent> : IEventBusIntegrationEvent 
        where TIntegrationEvent: IEventBusEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IEventBusIntegrationEvent
    {
    }
}
