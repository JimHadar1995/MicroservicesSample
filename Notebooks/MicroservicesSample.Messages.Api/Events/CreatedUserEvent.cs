using System;
using MicroservicesSample.Common.EventBus;

namespace MicroservicesSample.Messages.Api.Events
{
    public class CreatedUserEvent : IEventBusEvent
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
