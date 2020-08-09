using System;
using MicroservicesSample.Common.EventBus;

namespace MicroservicesSample.Identity.Dto
{
    public class UserDto : IEventBusEvent
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Description { get; set; }
        public RoleDto Role { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
