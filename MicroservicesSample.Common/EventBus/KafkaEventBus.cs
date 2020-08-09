using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MicroservicesSample.Common.EventBus
{
    /// <summary>
    /// 
    /// </summary>
    public class KafkaEventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly KafkaOptions _kafkaOptions;
        private readonly ProducerConfig _producerConfig;
        private readonly ConsumerConfig _consumerConfig;
        private CancellationTokenSource? _cts;

        private readonly ConcurrentDictionary<string, Tuple<Type, Type>>
            _subscribeEvents = new ConcurrentDictionary<string, Tuple<Type, Type>>();

        /// <inheritdoc />
        public KafkaEventBus(
            IOptions<KafkaOptions> kafkaOptions,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _kafkaOptions = kafkaOptions.Value;
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServer
            };
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServer, 
                EnableAutoCommit = false, 
                GroupId = _kafkaOptions.GroupId,
                ClientId = _kafkaOptions.ClientId
            };
        }

        /// <inheritdoc />
        public async Task PublishAsync<TEvent>(string topicName, string eventName, TEvent @event)
            where TEvent : IEventBusEvent
        {
            try
            {
                using var producerBuilder = new ProducerBuilder<string, string>(_producerConfig).Build();
                await producerBuilder.ProduceAsync(topicName,
                    new Message<string, string> {Key = eventName, Value = JsonConvert.SerializeObject(@event)});
            }
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
                //
            }
        }

        /// <inheritdoc />
        public Task Subscribe<TEvent, THandler>(
            string eventName,
            CancellationToken token)
            where TEvent : IEventBusEvent
            where THandler : IEventBusIntegrationEvent<TEvent>
        {
            if (!_subscribeEvents.ContainsKey(eventName))
            {
                _subscribeEvents.TryAdd(eventName, new Tuple<Type, Type>(typeof(TEvent), typeof(THandler)));
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task UnSubScribe<TEvent, THandler>(
            string eventName)
            where TEvent : IEventBusEvent
            where THandler : IEventBusIntegrationEvent<TEvent>
        {
            if (_subscribeEvents.ContainsKey(eventName))
            {
                _subscribeEvents.Remove(eventName, out _);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void StartConsume(string topicName)
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                return;
            }

            _cts = new CancellationTokenSource();
            StartConsume(_serviceProvider, topicName, _cts.Token);
        }

        public void StopConsume()
        {
            if (_cts == null)
                return;
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        public void Dispose()
        {
            StopConsume();
        }

        private void StartConsume(
            IServiceProvider serviceProvider,
            string topicName,
            CancellationToken token)
        {
            Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        
                        using var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
                        consumer.Subscribe(topicName);

                        while (!token.IsCancellationRequested)
                        {
                            try
                            {
                                var consumeResult = consumer.Consume(token);
                                if(consumeResult == null)
                                    continue;
                                
                                string eventName = consumeResult.Key;
                                if (_subscribeEvents.ContainsKey(eventName))
                                {
                                    using var scope = serviceProvider.CreateScope();
                                    var scopeSp = scope.ServiceProvider;
                                    var types = _subscribeEvents[eventName];
                                    var @event = JsonConvert.DeserializeObject(consumeResult.Value, types.Item1);
                                    var handler = scopeSp.GetRequiredService(types.Item2);
                                    MethodInfo magicMethod = types.Item2.GetMethod("Handle")!;
                                    var task = (Task)magicMethod.Invoke(handler, new[] {@event})!;
                                    await task!.ConfigureAwait(false);
                                    // consumer.Commit(consumeResult);
                                }

                                consumer.Commit();
                            }
                            catch(Exception ex)
                            {
                                
                            }
                        }
                        consumer.Close();

                        
                    }
#pragma warning disable 168
                    catch (Exception ex)
#pragma warning restore 168
                    {
                    }
                }, token,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        class tmp
        {
            public string Description { get; set; }
        }
    }
}
