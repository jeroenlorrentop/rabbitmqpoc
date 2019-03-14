using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Features.Metadata;
using MassTransit;

namespace RabbitPoc.Core
{
    public interface IRabbitMqBusFactory
    {
        IBusControl Bus { get; }
    }

    public class RabbitMqBusFactory : IRabbitMqBusFactory
    {
        private readonly IBusSettings _settings;
        private readonly IEnumerable<Meta<IConsumer,ConsumerMetaData>> _consumerMeta;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IQueueNameParser _queueNameParser;

        public RabbitMqBusFactory(
            IBusSettings settings,
            IEnumerable<Meta<IConsumer,ConsumerMetaData>> consumerMeta,
            ILifetimeScope lifetimeScope,
            IQueueNameParser queueNameParser
            )
        {
            _settings = settings;
            _consumerMeta = consumerMeta;
            _lifetimeScope = lifetimeScope;
            _queueNameParser = queueNameParser;


            SetupBus();
        }

        private void SetupBus()
        {
            _bus = new Lazy<IBusControl>(() =>
                MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(_settings.RabbitMqServer), hst =>
                    {
                        hst.Username(_settings.RabbitMqUsername);
                        hst.Password(_settings.RabbitMqPassword);
                    });

                    foreach (var group in _consumerMeta.GroupBy(x => x.Metadata.QueueNameTemplate))
                    {
                        var queueName = _queueNameParser.Parse(group.Key);
                        
                        cfg.ReceiveEndpoint(queueName, endpointConfig =>
                        {
                            foreach (var meta in group)
                            {
                                endpointConfig.Consumer(meta.Metadata.ConsumerType, type => _lifetimeScope.Resolve(type));
                            }
                        });
                    }
                }));
        }

        private Lazy<IBusControl> _bus;
        
        public IBusControl Bus => _bus.Value;


    }
}