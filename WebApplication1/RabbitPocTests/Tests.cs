using System;
using System.Linq;
using RabbitPoc.Core;
using Xunit;

namespace RabbitPocTests
{
    public class Tests
    {
        [Queue("MyTestQueueNameWithAVariable-{MachineName}-{Guid}")]
        public class TestClassForQueueAttribute
        {
            
        }

        public class DerivedTestClass : TestClassForQueueAttribute
        {
            
        }
        
        [Queue("SomeRandomQueueName")]
        public class DerivedTestClassWithOwnAttribute : TestClassForQueueAttribute
        {
            
        }
        
        [Fact]
        public void MetaIsComplete()
        {
            var meta = ConsumerMetaData.Create(typeof(TestClassForQueueAttribute));
            
            Assert.Equal("MyTestQueueNameWithAVariable-{MachineName}-{Guid}", meta.QueueNameTemplate);
            Assert.Equal(typeof(TestClassForQueueAttribute), meta.ConsumerType);
         
        }
        
        [Fact]
        public void MetaIsInheritedFromBase()
        {
            var meta = ConsumerMetaData.Create(typeof(DerivedTestClass));
            
            Assert.Equal("MyTestQueueNameWithAVariable-{MachineName}-{Guid}", meta.QueueNameTemplate);
            Assert.Equal(typeof(DerivedTestClass), meta.ConsumerType);
         
        }
         
        [Fact]
        public void MetaIsOverridenOnSubType()
        {
            var meta = ConsumerMetaData.Create(typeof(DerivedTestClassWithOwnAttribute));
            
            Assert.Equal("SomeRandomQueueName", meta.QueueNameTemplate);
            Assert.Equal(typeof(DerivedTestClassWithOwnAttribute), meta.ConsumerType);
         
        }
        
       
    }
   
}