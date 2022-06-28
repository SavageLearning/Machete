using System.Collections.Generic;

namespace Machete.Api.ViewModel
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
