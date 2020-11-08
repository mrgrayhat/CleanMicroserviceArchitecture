using System.Collections.Generic;
using System.Threading.Tasks;
using SaeedRezayi.LogModule.Models;

namespace SaeedRezayi.LogModule.Services
{
    public interface ILogService
    {
        //string GetLogs();
        Task<PagedLogsListViewModel> FilterLogs(string term, string field, int pageNumber, string sortOrder = "Desc", int recordsPerPage = 10, LogLevels logLevel = LogLevels.ALL);

        Task Add(LogInfo log, LogLevels logLevel);
        Task<LogInfo> Remove(string id, LogLevels logLevels = LogLevels.ALL);
        Task<LogInfo> Get(string id, LogLevels logLevels = LogLevels.ALL);
        Task<PagedLogsListViewModel> GetLogs(int pageNumber, string sortByField,
            string sortOrder, int recordsPerPage = 10,
            LogLevels logLevel = LogLevels.ALL);
    }
}
