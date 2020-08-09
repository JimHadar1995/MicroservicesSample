using System.Threading.Tasks;
using Consul;

namespace MicroservicesSample.Common.Consul
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConsulServicesRegistry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<AgentService> GetAsync(string name);
    }
}
