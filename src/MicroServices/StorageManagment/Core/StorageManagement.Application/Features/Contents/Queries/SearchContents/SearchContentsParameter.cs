using StorageManagement.Application.Parameters;

namespace StorageManagement.Application.Features.Contents.Queries.SearchContents
{
    public class SearchContentsParameter : RequestParameter
    {
        public string Text { get; set; }
        public string SortOrder { get; set; }
    }
}
