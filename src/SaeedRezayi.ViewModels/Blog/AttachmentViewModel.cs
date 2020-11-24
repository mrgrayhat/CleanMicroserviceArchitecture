using System;
using SaeedRezayi.DomainClasses.Blog.Posts;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.ViewModels.Blog
{
    public class AttachmentViewModel : CommonEntityProperties<Guid>
    {

        public string Name { get; set; }
        public string Password { get; set; }
        public long Size { get; set; }
        public int TotalDownloads { get; set; }
        public bool IsValid { get; set; } = true;
        public string Url { get; set; }
        public int UserId { get; set; }
        // [JsonIgnore]
        // public UserViewModel User { get; set; }

        public static implicit operator AttachmentViewModel(AttachmentInfo attachmentInfo)
        {
            if (attachmentInfo == null)
                return null;
            return new AttachmentViewModel
            {
                Id = attachmentInfo.Id,
                Name = attachmentInfo.Name,
                IsValid = attachmentInfo.IsValid,
                Size = (attachmentInfo.Size),
                TotalDownloads = attachmentInfo.TotalDownloads,
                Url = attachmentInfo.Url,
                UpdatedAt = attachmentInfo.UpdatedAt,
                CreatedAt = attachmentInfo.CreatedAt,
                UserId = attachmentInfo.UserId,
                // User = attachmentInfo.User,
                Password = attachmentInfo.Password
            };
        }
        public static implicit operator AttachmentInfo(AttachmentViewModel attachmentViewModel)
        {
            if (attachmentViewModel == null)
                return null;
            return new AttachmentInfo
            {
                Name = attachmentViewModel.Name,
                IsValid = attachmentViewModel.IsValid,
                Size = (attachmentViewModel.Size),
                TotalDownloads = attachmentViewModel.TotalDownloads,
                Url = attachmentViewModel.Url,
                UpdatedAt = attachmentViewModel.UpdatedAt,
                CreatedAt = attachmentViewModel.CreatedAt,
                UserId = attachmentViewModel.UserId,
                // User = attachmentViewModel.User,
                Password = attachmentViewModel.Password
            };
        }
    }
}
