﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BlogModule.Application.Events.Posts
{
    public class PostCreatedEvent : INotification
    {
        public PostCreatedEvent(DateTime requestDate, string client)
        {
            RequestDate = requestDate;
            Client = client;
        }
        public string Client { get; }
        public DateTime RequestDate { get; }
    }

    public class PostCreatedEmailSenderHandler : INotificationHandler<PostCreatedEvent>
    {
        readonly ILogger<PostCreatedEmailSenderHandler> _logger;
        public PostCreatedEmailSenderHandler(ILogger<PostCreatedEmailSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
        {
            // IMessageSender.Send($"blog post's requested at {notification.RequestDate} from client {notification.Client}");
            return Task.CompletedTask;
        }
    }
    public class PostCreatedLogSenderHandler : INotificationHandler<PostCreatedEvent>
    {
        readonly ILogger<PostCreatedLogSenderHandler> _logger;
        public PostCreatedLogSenderHandler(ILogger<PostCreatedLogSenderHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning($"new blog post created at {notification.RequestDate} from client {notification.Client}");
            return Task.CompletedTask;
        }
    }
}