using System;

namespace MicroservicesSample.Common.EventBus
{
    /// <summary>
    /// 
    /// </summary>
    public class KafkaOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string BootstrapServer { get; set; } = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string GroupId { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// Таймаут на время отправки каждого сообщения в брокер. В секундах
        /// </summary>
        public int TimeoutSeconds { get; set; } = 10;
    }
}
