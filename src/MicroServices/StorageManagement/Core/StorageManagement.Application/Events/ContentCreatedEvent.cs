using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace StorageManagement.Application.Events
{
    public class ContentCreatedEvent : INotification
    {
        public ContentCreatedEvent(DateTime requestDate, string client, string hash, string path)
        {
            RequestDate = requestDate;
            Client = client;
            VerifiedHash = hash;
            Path = path;
        }
        public string Client { get; }
        public string VerifiedHash { get; }
        public string Path { get; }
        public DateTime RequestDate { get; }
    }

    public class ContentCreatedEmailSenderHandler : INotificationHandler<ContentCreatedEvent>
    {
        private readonly ILogger<ContentCreatedEmailSenderHandler> _logger;
        public ContentCreatedEmailSenderHandler(ILogger<ContentCreatedEmailSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(ContentCreatedEvent notification, CancellationToken cancellationToken)
        {
            // IMessageSender.Send("new storage item created in {Path}, at {RequestDate} from client {Client}, verified hash is {VerifiedHash}", notification.Path, notification.RequestDate, notification.Client, notification.VerifiedHash);
            return Task.CompletedTask;
        }
    }
    public class ContentCreatedLogSenderHandler : INotificationHandler<ContentCreatedEvent>
    {
        private readonly ILogger<ContentCreatedLogSenderHandler> _logger;
        public ContentCreatedLogSenderHandler(ILogger<ContentCreatedLogSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(ContentCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("new storage item created in {Path}, at {RequestDate} from client {Client}, verified hash is {VerifiedHash}", notification.Path, notification.RequestDate, notification.Client, notification.VerifiedHash);
            return Task.CompletedTask;
        }
    }
}