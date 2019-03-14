using System;

namespace RabbitPoc.Core
{
    public class QueueNameParser : IQueueNameParser
    {
        private readonly object _values;
        
        public QueueNameParser(IBusSettings busSettings)
        {
            _values = new
            {
                Environment.MachineName,
                busSettings.DefaultQueue
            };
        }
       
        public string Parse(string template)
        {
            return SmartFormat.Smart.Format(template, _values);
        }
    }
}