namespace SaeedRezayi.LogModule.Models
{
    public interface ILogDatabaseSettings
    {
        string Provider { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string ErrorLogCollectionName { get; set; }
        string InformationLogCollectionName { get; set; }
        int MaxThreads { get; set; }
    }
}