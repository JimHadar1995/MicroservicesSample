using MicroservicesSample.Common.EventBus;

namespace MicroservicesSample.Identity.Application.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDeletedEvent : IEventBusEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
