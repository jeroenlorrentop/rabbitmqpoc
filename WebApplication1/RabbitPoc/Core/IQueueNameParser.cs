namespace RabbitPoc.Core
{
    public interface IQueueNameParser
    {
        string Parse(string template);
    }
}