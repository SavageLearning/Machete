
namespace Machete.Api.ViewModel
{
    /// <summary>
    /// Machete Api request parameters object
    /// </summary>
    public class ApiRequestParams
    {
        /// <summary>
        /// Some controllers need to return all records, like Configs.
        /// Use this to pass to MacheteApi2Controller.cs
        /// </summary>
        internal bool AllRecords = false;

        /// <summary>
        /// Calculated number of records to skip based on the
        /// pageNumber and pageSize properties
        /// </summary>
        internal int Skip { get => pageNumber == 1 ? 0 : pageSize * (pageNumber - 1); }

        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        /// <summary>
        /// page number for paginated results
        /// </summary>
        public int pageNumber { get; set; } = 1;

        /// <summary>
        /// the number of pages requested in results.
        /// Max 50, min 10
        /// </summary>
        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        /// <summary>
        /// The field name used sort the results
        /// </summary>
        public string sortField { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool sortDesc { get; set; } = false;

        /// <summary>
        /// Optional query param, used to filter results
        /// </summary>
        public string search { get; set; }

        /// <summary>
        /// Optional query param, used to filter worker records
        /// </summary>
        public int? dwccardnum { get; set; }

        /// <summary>
        /// Optional query param, used to filter worker records
        /// </summary>
        public int? memberStatusId { get; set; }
    }
}
