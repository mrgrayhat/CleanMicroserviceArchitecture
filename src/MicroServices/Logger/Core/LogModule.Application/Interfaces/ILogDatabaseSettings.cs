
namespace LogModule.Application.DTOs.Interfaces
{
    public interface ILogDatabaseSettings
    {
        string Provider { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }

}
