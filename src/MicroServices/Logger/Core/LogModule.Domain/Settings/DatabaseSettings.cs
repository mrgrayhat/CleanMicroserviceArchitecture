
namespace LogModule.Domain.Settings
{
    public class LogDatabaseSettings : ILogDatabaseSettings
    {
        public string Provider { get; set; } = "MongoDb";
        public string DatabaseName { get; set; } = "ApplicationLogs";
        public string CollectionName { get; set; } = "Logs";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017/ApplicationLogs";

    }
    public interface ILogDatabaseSettings
    {
        public string Provider { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
    }
}