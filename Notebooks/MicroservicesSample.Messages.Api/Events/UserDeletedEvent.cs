using MicroservicesSample.Common.EventBus;

namespace MicroservicesSample.Messages.Api.Events
{
    public class UserDeletedEvent : IEventBusEvent
    {
        public string UserId { get; set; }
    }
}
