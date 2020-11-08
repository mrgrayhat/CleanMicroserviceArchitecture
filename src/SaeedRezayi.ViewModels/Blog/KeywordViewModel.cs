using System.Collections.Generic;
using System.Linq;
using SaeedRezayi.DomainClasses.Blog.Posts;

namespace SaeedRezayi.ViewModels.Blog
{
    public class KeywordViewModel
    {
        public KeywordViewModel()
        {
            Posts = new HashSet<PostViewModel>();
        }
        public int Id { get; set; }
        public string Title { get; set; }


        public virtual ICollection<PostViewModel> Posts { get; set; }

        public static implicit operator KeywordViewModel(KeywordInfo keywordInfo)
        {
            if (keywordInfo == null)
                return null;
            return new KeywordViewModel
            {
                Id = keywordInfo.Id,
                Title = keywordInfo.Title,
                //Posts = keywordInfo.PostKeywords?
                //.Select(x => (PostViewModel)x.Post)
                //.ToList()
            };
        }
        public static implicit operator KeywordInfo(KeywordViewModel keywordViewModel)
        {
            if (keywordViewModel == null)
                return null;
            return new KeywordInfo
            {
                Id = keywordViewModel.Id,
                Title = keywordViewModel.Title
            };
        }
    }
}
