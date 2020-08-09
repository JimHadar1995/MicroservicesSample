using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesSample.Common.Consul
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsulOptions
    {

        public bool Enabled { get; set; }

        public string Url { get; set; } = string.Empty;

        public string Service { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public int Port { get; set; }

        public bool PingEnabled { get; set; }

        public string PingEndpoint { get; set; } = string.Empty;

        public int PingInterval { get; set; }

        public int RemoveAfterInterval { get; set; }

        public int RequestRetries { get; set; }

        public bool SkipLocalhostDockerDnsReplace { get; set; }
    }
}
