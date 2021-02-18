using StorageManagement.Application.Parameters;

namespace StorageManagement.Application.Features.Contents.Queries.GetAllContents
{
    public class GetAllContentsParameter : RequestParameter
    {
        public string SortOrder { get; set; }
    }
}
