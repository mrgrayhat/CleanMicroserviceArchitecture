using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.DomainClasses.Blog.JoiningTables;
using SaeedRezayi.DomainClasses.Blog.Posts;
using SaeedRezayi.DomainClasses.Blog.Posts.Locales;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.ViewModels.Blog
{
    ///<summary>
    /// post response model
    ///</summary>
    public class PostViewModel : CommonEntityProperties<int>
    {
        public PostViewModel()
        {
            PostLocales = new HashSet<PostLocaleViewModel>();
            Attachments = new HashSet<AttachmentViewModel>();
            Tags = new HashSet<TagViewModel>();
            Keywords = new HashSet<KeywordViewModel>();
        }

        //public double Rating { get; set; }
        [JsonIgnore]
        public new DateTimeOffset? UpdatedAt { get; set; }
        public string Thumbnail { get; set; }
        public bool IsArchive { get; set; } = false;
        public bool IsPublic { get; set; } = true;

        [JsonIgnore]
        public long Visits { get; set; }
        public int AuthorId { get; set; }
        [JsonIgnore]
        public virtual UserInfo Author { get; set; }
        public int? CategoryId { get; set; }
        public virtual CategoryViewModel Category { get; set; }
        public ICollection<PostLocaleViewModel> PostLocales { get; set; }
        public ICollection<TagViewModel> Tags { get; set; }
        public ICollection<AttachmentViewModel> Attachments { get; set; }
        public ICollection<KeywordViewModel> Keywords { get; set; }
        //public ICollection<CommentViewModel> Comments { get; set; }

        public static implicit operator PostViewModel(PostInfo postInfo)
        {
            if (postInfo == null)
            {
                return null;
            }
            return new PostViewModel
            {
                Id = postInfo.Id,
                Author = postInfo.Author,
                IsArchive = postInfo.IsArchive,
                IsPublic = postInfo.IsPublic,
                AuthorId = postInfo.AuthorId,
                CreatedAt = postInfo.CreatedAt,
                UpdatedAt = postInfo.UpdatedAt,
                CategoryId = postInfo.CategoryId,
                Thumbnail = postInfo.Thumbnail,
                Visits = postInfo.Visits,
                Category = postInfo.Category ?? null,
                PostLocales = postInfo.Locales?.Select(x =>
                {
                    return (PostLocaleViewModel)x;
                }).ToList(),
                Tags = postInfo.PostTags?.Select(x =>
                {
                    return (TagViewModel)x.Tag;
                }).ToList(),
                Attachments = postInfo.PostAttachments?.Select(x =>
                {
                    return (AttachmentViewModel)x.Attachment;
                }).ToList(),
                Keywords = postInfo.PostKeywords?.Select(x =>
                {
                    return (KeywordViewModel)x.Keyword;
                })
                .ToList()
                //Comments = postInfo.PostComments?.Select(x=>(CommentViewModel)x.Comment).ToList()
            };
        }
        public static implicit operator PostInfo(PostViewModel postViewModel)
        {
            if (postViewModel == null)
            {
                return null;
            }

            PostInfo postInfo = new PostInfo
            {
                Id = postViewModel.Id,
                IsArchive = postViewModel.IsArchive,
                Visits = postViewModel.Visits,
                Thumbnail = postViewModel.Thumbnail,
                CategoryId = postViewModel.CategoryId,
                CreatedAt = postViewModel.CreatedAt,
                UpdatedAt = postViewModel.UpdatedAt,
                Category = postViewModel.Category,
                IsPublic = postViewModel.IsPublic,
                AuthorId = postViewModel.AuthorId,
                Author = postViewModel.Author,
                Locales = postViewModel.PostLocales?
                .Select(x => (PostLocaleInfo)x).ToList()
            };
            // TODO: Why this does'nt work?!
            //postInfo.PostKeywords = postInfo.PostKeywords?
            //    .Select(c => new PostKeyword
            //    { KeywordId = c.KeywordId, PostId = c.PostId })
            //    .ToList();

            // TODO: Find Better way to Map n*n entities data
            foreach (var item in postViewModel.Keywords)
                postInfo.PostKeywords.Add(new PostKeyword { Keyword = item });
            foreach (var item in postViewModel.Tags)
                postInfo.PostTags.Add(new PostTag { Tag = item });
            foreach (var item in postViewModel.Attachments)
                postInfo.PostAttachments.Add(new PostAttachment { Attachment = item });

            return postInfo;
        }
    }
}
