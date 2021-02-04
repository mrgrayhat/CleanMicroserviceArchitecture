using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LogModule.Application.Features.Logs.Queries.GetAllLogs
{
    /// <summary>
    /// Log ViewModel Response
    /// </summary>
    public class GetAllLogsViewModel
    {
        public string Id { get; set; }
        public DateTime Timestamp
        {
            get;
            set;
        }
        public string Level { get; set; }
        public string Renderings { get; set; }
        public string Exception { get; set; }
        [AutoMapper.IgnoreMap]
        [JsonIgnore]
        public string MessageTemplate { get; set; }
        public string RenderedMessage { get; set; }
        public Dictionary<object, object> Properties { get; set; }
        public string UtcTimestamp { get; set; }
    }
}