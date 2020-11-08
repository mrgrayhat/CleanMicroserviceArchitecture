using System.Collections.Generic;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.DomainClasses.Blog.Posts
{
    public class CategoryInfo: CommonEntityProperties<int>
    {
        public CategoryInfo()
        {
            Posts = new HashSet<PostInfo>();
            SubCategories=new HashSet<CategoryInfo>();
        }
        public string Title { get; set; }

        /// <summary>
        /// shot text describe this categoey
        /// </summary>
        public string Description { get; set; }

        public int? ParentId { get; set; }
        public virtual CategoryInfo Parent { get; set; }
        /// <summary>
        /// Childs
        /// </summary>
        public virtual ICollection<CategoryInfo> SubCategories { get; set; }

        /// <summary>
        /// posts in this category
        /// </summary>
        public virtual ICollection<PostInfo> Posts { get; set; }
    }
}
