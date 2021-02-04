using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BlogModule.Application.Events.Posts
{
    public class PostApprovalPendingEvent : INotification
    {
        public PostApprovalPendingEvent(DateTime requestDate, string user,string postTitle)
        {
            RequestDate = requestDate;
            User = user;
            PostTitle = postTitle;
        }
        public string User { get; }
        public string PostTitle { get; }
        public DateTime RequestDate { get; }
    }
    public class PostApprovalPendingEmailSenderHandler : INotificationHandler<PostApprovalPendingEvent>
    {
        readonly ILogger<PostApprovalPendingEmailSenderHandler> _logger;
        public PostApprovalPendingEmailSenderHandler(ILogger<PostApprovalPendingEmailSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(PostApprovalPendingEvent notification, CancellationToken cancellationToken)
        {
            //IMessageSender.Send($"new blog post from user {notification.User} with title {notification.PostTitle} pending for approval at {notification.RequestDate}");

            return Task.CompletedTask;
        }
    }

    public class PostApprovalPendingLogSenderHandler : INotificationHandler<PostApprovalPendingEvent>
    {
        readonly ILogger<PostApprovalPendingLogSenderHandler> _logger;
        public PostApprovalPendingLogSenderHandler(ILogger<PostApprovalPendingLogSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(PostApprovalPendingEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning($"new blog post from user {notification.User} with title {notification.PostTitle} pending for approval at {notification.RequestDate}");

            return Task.CompletedTask;
        }
    }
}
