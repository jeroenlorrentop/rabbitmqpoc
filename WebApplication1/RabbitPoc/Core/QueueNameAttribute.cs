using System;

namespace RabbitPoc.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueueNameAttribute : Attribute
    {
        public string NameTemplate { get;  }
        public QueueNameAttribute(string nameTemplate)
        {
            NameTemplate = nameTemplate;
        }
    }
}