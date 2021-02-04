using System;
using System.Collections.Generic;
using System.Text.Json;
using MongoDB.Bson;

namespace LogModule.Application.DTOs.Log
{
    public class LogResponse
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

        public static implicit operator LogResponse(Domain.Entities.Log log)
        {
            if (log == null)
                return null;

            return new LogResponse
            {
                Id = log.Id,
                Level = log.Level,
                MessageTemplate = log.MessageTemplate,
                RenderedMessage = log.RenderedMessage,
                Renderings = log.Renderings == null ? "" : log.Renderings.ToJson(),
                Properties = log.Properties,
                Timestamp = log.Timestamp,
                UtcTimestamp = JsonSerializer
                .Serialize(BsonTypeMapper.MapToDotNetValue(log.UtcTimestamp))
            };
        }

    }
}
