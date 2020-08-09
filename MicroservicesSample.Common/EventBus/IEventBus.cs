using System;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.Common.EventBus;

namespace MicroservicesSample.Common.EventBus
{
    public interface IEventBus : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="event"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        Task PublishAsync<TEvent>(string topicName, string eventName, TEvent @event)
            where TEvent : IEventBusEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        Task Subscribe<TEvent, THandler>(
            string eventName,
            CancellationToken token = default)
            where TEvent : IEventBusEvent
            where THandler : IEventBusIntegrationEvent<TEvent>;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="eventName"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        Task UnSubScribe<TEvent, THandler>(
            string eventName)
            where TEvent : IEventBusEvent
            where THandler : IEventBusIntegrationEvent<TEvent>;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        void StartConsume(string topicName);

        /// <summary>
        /// 
        /// </summary>
        void StopConsume();
    }
}
