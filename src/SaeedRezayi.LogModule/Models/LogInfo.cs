using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SaeedRezayi.LogModule.Models
{
    public class LogInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Timestamp { get; set; }
        [BsonElement]
        public string Level { get; set; }
        [BsonIgnoreIfNull]
        [JsonIgnore]
        public BsonDocument Renderings { get; set; }
        [BsonElement]
        [BsonIgnoreIfNull]
        public string Exception { get; set; }
        [BsonElement]
        public string MessageTemplate { get; set; }
        [BsonElement]
        [BsonIgnoreIfNull]
        public string RenderedMessage { get; set; }
        //[JsonIgnore]
        public Dictionary<object, object> Properties { get; set; }
        public BsonString UtcTimestamp { get; set; }
    }

}
