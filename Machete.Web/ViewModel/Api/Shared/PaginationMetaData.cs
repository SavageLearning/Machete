using System.Collections.Generic;

namespace Machete.Web.ViewModel.Api
{
    /// <summary>
    /// Pagination metadata viewmodel
    /// </summary>
    public class PaginationMetaData
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
        public int recordCount { get; set; }
    }
}
