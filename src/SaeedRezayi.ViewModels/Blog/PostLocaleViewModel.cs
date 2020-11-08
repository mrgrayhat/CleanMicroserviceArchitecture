using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SaeedRezayi.DomainClasses.Blog.Posts.Locales;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.ViewModels.Blog
{
    public class PostLocaleViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// The title of the post
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string Title { get; set; }
        /// <summary>
        /// The post body
        /// </summary>
        [Required]
        [MaxLength(10000)]
        public string Content { get; set; }

        [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "Slug format not valid.")]
        [StringLength(160)]
        public string Slug { get; set; }

        [Required]
        [MaxLength(5)]
        public string LocaleCultureCode { get; set; }
        public int LocaleCultureId { get; set; }
        public CultureInfo LocaleCulture { get; set; }

        public int PostId { get; set; }
        //public PostViewModel Post { get; set; }

        public static implicit operator PostLocaleViewModel(PostLocaleInfo postInfoLocale)
        {
            if (postInfoLocale == null)
            {
                return null;
            }
            return new PostLocaleViewModel
            {
                Id = postInfoLocale.Id,
                //Post = postInfoLocale.Post,
                PostId = postInfoLocale.PostId,
                Content = postInfoLocale.Content,
                Slug = postInfoLocale.Slug,
                Title = postInfoLocale.Title,
                LocaleCultureId = postInfoLocale.CultureId,
                LocaleCulture = postInfoLocale.LocalCulture
            };
        }

        public static implicit operator PostLocaleInfo(PostLocaleViewModel postLocaleViewModel)
        {
            if (postLocaleViewModel == null)
            {
                return null;
            }
            return new PostLocaleInfo
            {
                //Post=postLocaleViewModel.Post,
                //PostId = postLocaleViewModel.PostId,
                Title = postLocaleViewModel.Title,
                Content = postLocaleViewModel.Content,
                Slug = postLocaleViewModel.Slug,
                CultureId = postLocaleViewModel.LocaleCultureId,
                LocalCulture = postLocaleViewModel.LocaleCulture
            };
        }

    }
}
