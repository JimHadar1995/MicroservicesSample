using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;

namespace MicroservicesSample.ApiGateway.Code
{
    /// <inheritdoc />
    public class AuthHeadersInterceptor : Interceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <inheritdoc />
        public AuthHeadersInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var metadata = new Metadata
            {
                {"Authorization", $"Bearer <JWT_TOKEN>"}
            };
            
            var callOption = context.Options.WithHeaders(metadata);
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, callOption);
        
            return base.AsyncUnaryCall(request, context, continuation);
        }
    }
}
