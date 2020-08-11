using System;
using MicroservicesSample.Common.EventBus;
using MicroservicesSample.Identity.Dto.Implementations;

namespace MicroservicesSample.Identity.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDto : IEventBusEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; } = string.Empty;
        
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public RoleDto Role { get; set; } = null!;
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        
    }
}
