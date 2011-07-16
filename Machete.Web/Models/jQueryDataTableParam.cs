using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Models
{
    /// <summary>
    /// Class that encapsulates most common parameters sent by DataTables plugin
    /// </summary>
    public class jQueryDataTableParam
    {
        /// <summary>
        /// Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }
        public string sSearch_2 { get; set; }
        public string sSearch_5 { get; set; }
        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortCol_0 { get; set;}

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_0 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_1 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_2 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_3 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_4 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_5 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_6 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_7 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_8 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_9 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_10 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_11 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_12 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_13 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_14 { get; set; }
        /// <summary>
        /// name of indexed column
        /// </summary>
        public string mDataProp_15 { get; set; }
    }
}