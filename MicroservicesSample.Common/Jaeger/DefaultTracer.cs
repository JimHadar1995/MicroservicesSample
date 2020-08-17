using System.Reflection;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using OpenTracing;

namespace MicroservicesSample.Common.Jaeger
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultTracer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ITracer Create()
            => new Tracer.Builder(Assembly.GetEntryAssembly()!.FullName)
                .WithReporter(new NoopReporter())
                .WithSampler(new ConstSampler(false))
                .Build();
    }
}
