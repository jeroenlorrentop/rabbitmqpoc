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
            var firstRun = SmartFormat.Smart.Format(template, _values);
            // 2 time parsing because DefaultQueue can have MachineName in it. 
            return SmartFormat.Smart.Format(firstRun,_values);
        }
    }
}