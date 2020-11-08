using SaeedRezayi.DomainClasses.Blog.Posts;

namespace SaeedRezayi.ViewModels.Blog
{
    public class TagViewModel
    {
        public TagViewModel()
        {

        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Visits { get; set; }


        public static implicit operator TagViewModel(TagInfo tagInfo)
        {
            if (tagInfo == null)
                return null;
            return new TagViewModel
            {
                Id = tagInfo.Id,
                Title = tagInfo.Title,
                Visits = tagInfo.Visits
            };
        }
        public static implicit operator TagInfo(TagViewModel tagViewModel)
        {
            if (tagViewModel == null)
                return null;
            return new TagInfo
            {
                Id = tagViewModel.Id,
                Title = tagViewModel.Title,
                Visits = tagViewModel.Visits
            };
        }
    }
}
