using System;
using System.Collections.Generic;
using System.Linq;

namespace RabbitPoc.Core
{
    public class ConsumerMetaData
    {
        public string QueueNameTemplate { get; set; }
        public Type ConsumerType { get; set; }

        public static ConsumerMetaData Create(Type type)
        {
            var meta = new ConsumerMetaData {ConsumerType = type};
            var att = type.GetCustomAttributes(typeof(QueueAttribute), true).FirstOrDefault();
            meta.QueueNameTemplate = att == null ? "{DefaultQueue}" : ((QueueAttribute) att).NameTemplate;

           return meta;
        }

        public IEnumerable<KeyValuePair<string,object>> ToKeyValuePairs()
        {
            var list = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(nameof(QueueNameTemplate), QueueNameTemplate), 
                new KeyValuePair<string, object>(nameof(ConsumerType), ConsumerType)
            };

            return list;
        }
    }
}