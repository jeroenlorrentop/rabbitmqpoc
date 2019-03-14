using System.Reflection;
using Autofac;
using MassTransit;
using Module = Autofac.Module;

namespace RabbitPoc.Core
{
    public class MessagingModule : Module
    {
        private readonly Assembly[] _consumerAssemblies;

        public MessagingModule(params Assembly[] consumerAssemblies)
        {
            _consumerAssemblies = consumerAssemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(_consumerAssemblies)
                .Where(x => typeof(IConsumer).IsAssignableFrom(x))
                .WithMetadata(x => ConsumerMetaData.Create(x).ToKeyValuePairs())
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterType<RabbitMqBusFactory>().As<IRabbitMqBusFactory>().SingleInstance();
            
            builder.RegisterConfigSettings<IBusSettings>();
            
            builder.RegisterType<QueueNameParser>().As<IQueueNameParser>().SingleInstance();
            
            builder.Register(x => x.Resolve<IRabbitMqBusFactory>().Bus)
                .As<IBusControl>()
                .As<IBus>();
        }
    }
}