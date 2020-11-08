
namespace SaeedRezayi.LogModule.Models
{
    public class LogDatabaseSettings : ILogDatabaseSettings
    {
        public string InformationLogCollectionName { get; set; } = "InformationLogs";
        public string ErrorLogCollectionName { get; set; } = "ErrorLogs";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017/Logs";
        public string DatabaseName { get; set; } = "Logs";
        public string Provider { get; set; } = "MongoDb";
        public int MaxThreads { get; set; } = 8;
    }
}
