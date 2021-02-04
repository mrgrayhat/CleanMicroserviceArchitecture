using LogModule.Application.Enums;
using LogModule.Application.Parameters;

namespace LogModule.Application.Features.Logs.Queries.GetAllLogs
{
    public class GetAllLogsParameter : RequestParameter
    {
        public LogLevels LogLevel { get; set; } = LogLevels.ALL;
        public string IP { get; internal set; }
    }
}
