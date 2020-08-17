namespace MicroservicesSample.Common.Jaeger
{
    /// <summary>
    /// 
    /// </summary>
    public class JaegerOptions
    {
        /// <summary>
        /// Включен ли
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Название сервиса
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Хост
        /// </summary>
        public string UdpHost { get; set; } = string.Empty;
        
        /// <summary>
        /// Порт
        /// </summary>
        public int UdpPort { get; set; }
        
        /// <summary>
        /// Максимальный размер пакета
        /// </summary>
        public int MaxPacketSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Sampler { get; set; } = string.Empty;
        
        /// <summary>
        /// 
        /// </summary>
        public double MaxTracesPerSecond { get; set; } = 5;
        /// <summary>
        /// 
        /// </summary>
        public double SamplingRate { get; set; } = 0.2;
    }
}
