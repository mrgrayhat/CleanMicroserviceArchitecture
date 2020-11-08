using System;
using System.Net;

namespace SaeedRezayi.Common.Messages
{
    public class MessageContract
    {
        public MessageStatusCodeTypes StatusCode { get; set; } = MessageStatusCodeTypes.ERROR;
        public string Message { get; set; }
        public object Parameter { get; set; }
        public Exception Exception { get; set; }

    }
    public enum MessageStatusCodeTypes
    {
        ACCEPTED = HttpStatusCode.Accepted,
        SUCCESS = HttpStatusCode.OK,
        CREATED = HttpStatusCode.Created,
        INPROGRESS = HttpStatusCode.Processing,
        ERROR = HttpStatusCode.InternalServerError,
        INVALID = HttpStatusCode.BadRequest,
        UNAUTHORIZED = HttpStatusCode.Unauthorized,

    }
}
