using System.Collections.Generic;

namespace Machete.Service.shared
{
    public class PagedResults<T>
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
        public int recordCount { get; set; }
        public IEnumerable<T> data { get; set; }
    }
}
