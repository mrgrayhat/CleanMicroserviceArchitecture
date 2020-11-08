using System.Collections.Generic;
using cloudscribe.Web.Pagination;

namespace SaeedRezayi.LogModule.Models
{
    public class PagedLogsListViewModel
    {
        public PagedLogsListViewModel()
        {
            Paging = new PaginationSettings();

        }
        public ICollection<LogInfo> Logs { get; set; }
        public PaginationSettings Paging { get; set; }

    }
}
