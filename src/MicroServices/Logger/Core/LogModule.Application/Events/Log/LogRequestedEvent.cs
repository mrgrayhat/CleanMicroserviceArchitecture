using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LogModule.Application.Events.Log
{
    public class LogRequestedEvent : INotification
    {
        public LogRequestedEvent(DateTime requestDate, string ip)
        {
            RequestDate = requestDate;
            IP = ip;
        }
        public string IP { get; }
        public DateTime RequestDate { get; }
    }
    public class LogRequestedEmailSenderHandler : INotificationHandler<LogRequestedEvent>
    {
        readonly ILogger<LogRequestedEmailSenderHandler> _logger;
        public LogRequestedEmailSenderHandler(ILogger<LogRequestedEmailSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(LogRequestedEvent notification, CancellationToken cancellationToken)
        {
            // IMessageSender.Send($"system logs requested at {notification.RequestDate} from ip {notification.IP}");
            return Task.CompletedTask;
        }
    }
    public class LogRequestedLogSenderHandler : INotificationHandler<LogRequestedEvent>
    {
        readonly ILogger<LogRequestedLogSenderHandler> _logger;
        public LogRequestedLogSenderHandler(ILogger<LogRequestedLogSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(LogRequestedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning($"system logs requested at {notification.RequestDate} from ip {notification.IP}");
            Console.WriteLine($"system logs requested at {notification.RequestDate} from ip {notification.IP}");
            return Task.CompletedTask;
        }
    }

}
