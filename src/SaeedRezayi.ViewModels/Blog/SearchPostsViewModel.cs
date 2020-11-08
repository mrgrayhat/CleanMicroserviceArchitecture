using System;
using System.ComponentModel.DataAnnotations;
using SaeedRezayi.ViewModels.Types;

namespace SaeedRezayi.ViewModels.Blog
{
    public class SearchPostsViewModel
    {
        public SearchPostsViewModel()
        {
            MaxNumberOfRows = 7;
        }

        [Display(Name = "جستجوی عبارت")]
        public string TextToFind { set; get; }
        [Display(Name = "تعداد ردیف بازگشتی")]
        [Required(ErrorMessage = "(*)")]
        [Range(1, 1000, ErrorMessage = "عدد وارد شده باید در بازه 1 تا 1000 تعیین شود")]
        public int MaxNumberOfRows { set; get; }
        [Display(Name = "فقط آرشیو شده‌ها")]
        public bool IsArchive { set; get; }
        [Display(Name = "ترتیب نمایش")]
        public SortingOrderTypes SortingOrder { get; set; } = SortingOrderTypes.Descending;

        public PagedPostsListViewModel PagedPostsList { set; get; }

    }
}
