using MicroservicesSample.Common.EventBus;

namespace MicroservicesSample.Identity.Application.Events
{
    public class UserDeletedEvent : IEventBusEvent
    {
        public string UserId { get; set; }
    }
}
