using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;

namespace MicroservicesSample.ApiGateway.Services
{
    internal static class GrpcCallerService
    {
        internal static async Task<TResponse> CallService<TClient, TResponse>(
            TClient client,
            Func<TClient, Task<TResponse>> func)
            where TClient: ClientBase 
        {
            // AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            // AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            
            try
            {
                return await func(client);
            }
            catch (RpcException e)
            {
                return default;
            }
            finally
            {
                // AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                // AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }
        }
        internal static async Task<TResponse> CallService<TResponse>(string urlGrpc, Func<GrpcChannel, Task<TResponse>> func)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            var channel = GrpcChannel.ForAddress(urlGrpc);

            /*
            using var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };

            */

            try
            {
                return await func(channel);
            }
            catch (RpcException e)
            {
                return default;
            }
            finally
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }


        }
        
        internal static async Task CallService(string urlGrpc, Func<GrpcChannel, Task> func)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            /*
            using var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };
            */

            var channel = GrpcChannel.ForAddress(urlGrpc);

            try
            {
                await func(channel);
            }
            catch (RpcException e)
            {
                
            }
            finally
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }
        }
    }
}
