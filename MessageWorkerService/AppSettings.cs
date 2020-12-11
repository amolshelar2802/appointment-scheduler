
namespace MessageWorkerService
{
    public class AppSettings
    {
        public Logging Logging { get; set;}

        public AzureServiceBusSettings AzureServiceBusSettings { get; set;}
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set;}

    }

    public class LogLevel
    {
        public string Default { get; set;}
        public string Microsoft { get; set;}
    }

    public class AzureServiceBusSettings
    {
        public string ConnectionString { get; set;}
        public string TopicName { get; set;}
        public string Subscription { get; set;}
    }

}
