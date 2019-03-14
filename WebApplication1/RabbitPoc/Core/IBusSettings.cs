namespace RabbitPoc.Core
{
    public interface IBusSettings
    {
        string RabbitMqUsername { get; }
        string RabbitMqPassword { get; }
        string RabbitMqServer { get; }

        string DefaultQueue { get; }
    }
}