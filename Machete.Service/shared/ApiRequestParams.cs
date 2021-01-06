using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Machete.Service.shared
{
    public class ApiRequestParams
    {
        private const int maxPageSize = 50;
        public int pageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int pageSize
        {
            get =>  _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public int skip { get => pageNumber == 1 ? 0 : pageSize * (pageNumber - 1); }
        public string sortField { get; set; }
        public bool sortDesc { get; set; } = false;
        public string search { get; set; }
        public int? dwccardnum { get; set; }
        public int? statusId { get; set; }
    }
}
