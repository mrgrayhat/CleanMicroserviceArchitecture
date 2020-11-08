
using System;
using System.Collections.Generic;
using System.Text.Json;
using MongoDB.Bson;

namespace SaeedRezayi.LogModule.Models
{
    public class LogViewModel
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Renderings { get; set; }
        public string Exception { get; set; }
        public string MessageTemplate { get; set; }
        public string RenderedMessage { get; set; }
        public Dictionary<object, object> Properties { get; set; }
        public string UtcTimestamp { get; set; }

        public static implicit operator LogViewModel(LogInfo logInfo)
        {
            if (logInfo == null)
            {
                return null;
            }
            return new LogViewModel
            {
                Id = logInfo.Id,
                Level = logInfo.Level,
                MessageTemplate = logInfo.MessageTemplate,
                RenderedMessage = logInfo.RenderedMessage,
                Renderings = logInfo.Renderings == null ? "" : logInfo.Renderings.ToJson(),
                Properties = logInfo.Properties,
                Timestamp = logInfo.Timestamp,
                UtcTimestamp = JsonSerializer
                .Serialize(BsonTypeMapper.MapToDotNetValue(logInfo.UtcTimestamp))
            };
        }

        //public static implicit operator LogInfo(LogViewModel logViewModel)
        //{
        //    if (logViewModel == null)
        //    {
        //        return null;
        //    }
        //    return new LogInfo
        //    {

        //    };
        //}
    }
}