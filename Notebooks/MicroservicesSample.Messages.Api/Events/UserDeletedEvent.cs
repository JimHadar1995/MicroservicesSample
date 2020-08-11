using System;
using MicroservicesSample.Common.EventBus;

namespace MicroservicesSample.Notebooks.Api.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDeletedEvent : IEventBusEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; } = String.Empty;
    }
}
