using System;
using System.Threading.Tasks;
using Autofac;
using MassTransit;
using RabbitPoc.Core;

namespace RabbitPoc
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = new ContainerBuilder();

            builder.RegisterModule(new MessagingModule(typeof(Program).Assembly));

            var container = builder.Build();

           var bus =  container.Resolve<IBusControl>();
           bus.StartAsync().Wait();

           for (int i = 0; i < 2000; i++)
           {
               bus.Publish(new TestConsumerMessage2(){DeliveryMessage = "I am a message for a heavy loaded queue"});
           }
           
           bus.Publish(new TestConsumerMessage(){DeliveryMessage = "I should be delivered to the priority queue"});
         
           Console.WriteLine(".....");
           Console.ReadLine();
        }
    }
    
    [Queue("{DefaultQueue}_PriorityDelivery")]
    public class ConsumerWithDifferentQueue : IConsumer<TestConsumerMessage>
    {
        public Task Consume(ConsumeContext<TestConsumerMessage> context)
        {
            var bla = context;
            Console.WriteLine(context.Headers.ToJson());
            
            Console.WriteLine(context.Message.DeliveryMessage);
            return Task.CompletedTask;
        }
    }
    
      
    public class ConsumerWithDefaultQueue : IConsumer<TestConsumerMessage2>
    {
        public Task Consume(ConsumeContext<TestConsumerMessage2> context)
        {
            var bla = context;
            Console.WriteLine(context.Headers.GetAll().ToJson());
            
            Console.WriteLine(context.Message.DeliveryMessage);
            return Task.CompletedTask;
        }
    }

    public class TestConsumerMessage2
    {
        public string DeliveryMessage { get; set; }
    }


    public class TestConsumerMessage
    {
        public string DeliveryMessage { get; set; }
    }
}