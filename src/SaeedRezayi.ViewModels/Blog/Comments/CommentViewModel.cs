using System;
using System.Collections.Generic;
using System.Text;
using SaeedRezayi.DomainClasses.Blog.Comments;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.ViewModels.Blog.Comments
{
    public class CommentViewModel : CommonEntityProperties<long>
    {
        public string User { get; set; }
        public string CommentContent { get; set; }
        public PostViewModel Post { get; set; }
        public bool IsArchive { get; set; } = false;

        public long? ParentId { get; set; }
        public CommentViewModel Parent { get; set; }

        public static implicit operator CommentViewModel(CommentInfo commentInfo)
        {
            if (commentInfo == null)
            {
                return null;
            }
            return new CommentViewModel
            {
                Id = commentInfo.Id,
                CommentContent = commentInfo.Content,
                Parent = commentInfo.Parent,
                ParentId = commentInfo.ParentId,
                IsArchive = commentInfo.IsArchive,
                CreatedAt = commentInfo.CreatedAt,
                Post = (PostViewModel)commentInfo.PostComments.Post,
                UpdatedAt = commentInfo.UpdatedAt,
                User = commentInfo.User
            };
        }
        public static implicit operator CommentInfo(CommentViewModel commentViewModel)
        {
            if (commentViewModel == null)
            {
                return null;
            }
            return new CommentInfo
            {
                Id = commentViewModel.Id,
                Content = commentViewModel.CommentContent,
                Parent = commentViewModel.Parent,
                ParentId = commentViewModel.ParentId,
                IsArchive = commentViewModel.IsArchive,
                CreatedAt = commentViewModel.CreatedAt,
                UpdatedAt = commentViewModel.UpdatedAt,
                User = commentViewModel.User
            };
        }
    }
}
