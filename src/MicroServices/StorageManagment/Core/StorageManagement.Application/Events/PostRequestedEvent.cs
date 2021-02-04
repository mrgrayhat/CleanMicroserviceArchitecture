using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using StorageManagement.Application.Interfaces.Repositories;

namespace StorageManagement.Application.Events
{
    public class ContentRequestedEvent : INotification
    {
        /// <summary>
        /// event
        /// </summary>
        /// <param name="requestDate"></param>
        /// <param name="contentId">unique id/key</param>
        /// <param name="client">reciever</param>
        public ContentRequestedEvent(DateTime requestDate, int contentId, string client)
        {
            RequestDate = requestDate;
            ContentId = contentId;
            Client = client;
        }
        public string Client { get; }
        public int ContentId { get; set; }
        public DateTime RequestDate { get; }
    }

    public class ContentDownloadedHandler : INotificationHandler<ContentRequestedEvent>
    {
        private readonly ILogger<ContentDownloadedHandler> _logger;
        private readonly IStorageRepositoryAsync _postRepository;
        public ContentDownloadedHandler(ILogger<ContentDownloadedHandler> logger, IStorageRepositoryAsync postRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        }

        public async Task Handle(ContentRequestedEvent notification, CancellationToken cancellationToken)
        {
            await _postRepository.HitDownload(notification.ContentId);
            _logger.LogInformation($"storage content with id {notification.ContentId} downloaded at {notification.RequestDate} from client {notification.Client}");
            //return Task.CompletedTask;
        }
    }
    public class ContentRequestedEmailSenderHandler : INotificationHandler<ContentRequestedEvent>
    {
        private readonly ILogger<ContentRequestedEmailSenderHandler> _logger;
        public ContentRequestedEmailSenderHandler(ILogger<ContentRequestedEmailSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(ContentRequestedEvent notification, CancellationToken cancellationToken)
        {
            // IMessageSender.Send($"storage content {notification.PostId} requested at {notification.RequestDate} from client {notification.Client}");
            return Task.CompletedTask;
        }
    }

    public class PostRequestedLogSenderHandler : INotificationHandler<ContentRequestedEvent>
    {
        private readonly ILogger<PostRequestedLogSenderHandler> _logger;
        public PostRequestedLogSenderHandler(ILogger<PostRequestedLogSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(ContentRequestedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Storage content with id {notification.ContentId} requested at {notification.RequestDate} from client {notification.Client}");
            return Task.CompletedTask;
        }
    }
}
