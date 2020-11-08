using System;
using System.Collections.Generic;
using System.Linq;
using SaeedRezayi.DomainClasses.Blog.Posts;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.ViewModels.Blog
{
    public class CategoryViewModel:CommonEntityProperties<int>
    {
        public CategoryViewModel()
        {
            //Posts = new HashSet<PostViewModel>();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TotalPosts { get; set; }

        public int? ParentId { get; set; }
        public virtual CategoryViewModel Parent { get; set; }
        // public virtual ICollection<PostViewModel> Posts { get; set; }

        public static implicit operator CategoryViewModel(CategoryInfo categoryInfo)
        {
            if (categoryInfo == null)
                return null;
            return new CategoryViewModel
            {
                Id = categoryInfo.Id,
                Title = categoryInfo.Title,
                CreatedAt = categoryInfo.CreatedAt,
                ParentId = categoryInfo.ParentId,
                UpdatedAt = categoryInfo.UpdatedAt,
                Description = categoryInfo.Description,
                Parent = categoryInfo.Parent,
                // Posts = categoryInfo.Posts.Select(p => (PostViewModel)p).ToList(),
                // TotalPosts = categoryInfo.Posts.Count()
            };
        }
        public static implicit operator CategoryInfo(CategoryViewModel categoryViewModel)
        {
            if (categoryViewModel == null)
                return null;
            return new CategoryInfo
            {
                Id = categoryViewModel.Id,
                Title = categoryViewModel.Title,
                ParentId=categoryViewModel.ParentId,
                CreatedAt = categoryViewModel.CreatedAt,
                UpdatedAt = categoryViewModel.UpdatedAt,
                Description = categoryViewModel.Description,
                Parent = categoryViewModel.Parent,
                // Posts = categoryViewModel.Posts.Select(p => (PostInfo)p).ToList()
            };
        }
    }
}
