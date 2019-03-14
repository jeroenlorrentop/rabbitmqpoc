using System;

namespace RabbitPoc.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueueAttribute : Attribute
    {
        public string NameTemplate { get;  }
        
        public QueueAttribute(string nameTemplate)
        {
            NameTemplate = nameTemplate;
        }
    }
}