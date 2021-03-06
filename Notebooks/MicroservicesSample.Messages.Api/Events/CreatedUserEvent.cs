using System;
using MicroservicesSample.Common.EventBus;

namespace MicroservicesSample.Notebooks.Api.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class CreatedUserEvent : IEventBusEvent
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
        public string Description { get; set; } = string.Empty;

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
