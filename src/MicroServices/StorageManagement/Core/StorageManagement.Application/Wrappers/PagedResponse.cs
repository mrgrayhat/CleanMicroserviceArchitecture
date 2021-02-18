
namespace StorageManagement.Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        /// <summary>
        /// number of page
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// max item per page
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// total items number
        /// </summary>
        public int Total { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize,int total)
        {
            this.Total = total;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
