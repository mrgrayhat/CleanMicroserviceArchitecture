using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Logger.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class LoggerController : BaseApiController
    {
        private readonly ILogger<LoggerController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggerController(ILogger<LoggerController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor)); ;
        }

        // GET: api/v1/<controller>
        [HttpGet]
        public ActionResult Index()
        {
            _logger.LogInformation($"Logger MicroService Built-in Controller, {nameof(Index)} action executed!");

            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] LogEvents body)
        {
            var eventsCount = body.Events.Length;
            var service = Request.Headers["X-Api-Key"].FirstOrDefault() ?? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation("Received batch of {count} log events from [{service}]. starting to write Logs: ", eventsCount, service);

            for (int i = 0; i < body.Events.Length; i++)
            {
                _logger.LogInformation("Level: {level}, Message: {message}",
                    body.Events[i].Level, body.Events[i].RenderedMessage);
            }

            _logger.LogInformation("Finished writing events");
            return Ok();
        }
    }

    public class LogEvent
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        //public string Renderings { get; set; }
        public string Exception { get; set; }
        //public string MessageTemplate { get; set; }
        public string RenderedMessage { get; set; }
        //public Dictionary<object, object> Properties { get; set; }
        //public object Properties { get; set; }
        public DateTime UtcTimestamp { get; set; }
    }
    public class LogEvents
    {
        public LogEvent[] Events { get; set; }
    }
}
