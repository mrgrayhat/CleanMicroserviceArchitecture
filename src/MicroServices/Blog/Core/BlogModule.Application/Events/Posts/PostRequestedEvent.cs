using System;
using System.Threading;
using System.Threading.Tasks;
using BlogModule.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BlogModule.Application.Events.Posts
{
    public class PostRequestedEvent : INotification
    {

        public PostRequestedEvent(DateTime requestDate,int postId, string client)
        {
            RequestDate = requestDate;
            PostId = postId;
            Client = client;
        }
        public string Client { get; }
        public int PostId { get; set; }
        public DateTime RequestDate { get; }
    }
    public class PostVisitedHandler : INotificationHandler<PostRequestedEvent>
    {
        readonly ILogger<PostVisitedHandler> _logger;
        readonly IPostRepositoryAsync _postRepository;
        public PostVisitedHandler(ILogger<PostVisitedHandler> logger, IPostRepositoryAsync postRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        }

        public async Task Handle(PostRequestedEvent notification, CancellationToken cancellationToken)
        {
            await _postRepository.Visit(notification.PostId);
            _logger.LogInformation($"blog post with id {notification.PostId} visited at {notification.RequestDate} from client {notification.Client}");
            //return Task.CompletedTask;
        }
    }
    public class PostRequestedEmailSenderHandler : INotificationHandler<PostRequestedEvent>
    {
        readonly ILogger<PostRequestedEmailSenderHandler> _logger;
        public PostRequestedEmailSenderHandler(ILogger<PostRequestedEmailSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(PostRequestedEvent notification, CancellationToken cancellationToken)
        {
            // IMessageSender.Send($"blog post {notification.PostId} requested at {notification.RequestDate} from client {notification.Client}");
            return Task.CompletedTask;
        }
    }
    public class PostRequestedLogSenderHandler : INotificationHandler<PostRequestedEvent>
    {
        readonly ILogger<PostRequestedLogSenderHandler> _logger;
        public PostRequestedLogSenderHandler(ILogger<PostRequestedLogSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(PostRequestedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"blog post with id {notification.PostId} requested at {notification.RequestDate} from client {notification.Client}");
            return Task.CompletedTask;
        }
    }
}
