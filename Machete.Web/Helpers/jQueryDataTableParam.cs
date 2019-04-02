#region COPYRIGHT
// File:     jQueryDataTableParam.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion

namespace Machete.Web.Helpers
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
        public string sSearch_0 { get; set; }
        public string sSearch_1 { get; set; }
        public string sSearch_2 { get; set; }
        public string sSearch_3 { get; set; }
        public string sSearch_4 { get; set; }
        public string sSearch_5 { get; set; }
        public string sSearch_6 { get; set; }
        public string sSearch_7 { get; set; }
        public string sSearch_8 { get; set; }
        public string sSearch_9 { get; set; }
        public string sSearch_10 { get; set; }
        public string sSearch_11 { get; set; }
        public string sSearch_12 { get; set; }
        public string sSearch_13 { get; set; }
        public string sSearch_14 { get; set; }
        public string sSearch_15 { get; set; }
        public string sSortDir_0 { get; set; }
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
        #region mDataProps_accessors
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
        #endregion
        #region FILTER_PARAMS
        /// <summary>
        /// filter table on a given date
        /// </summary>
        public string todaysdate { get; set; }
        /// <summary>
        /// filter table on a membership card number
        /// </summary>
        public string category { get; set; }
        public string dwccardnum { get; set; }
        public int? activityID { get; set; }
        public int? personID { get; set; }
        public string wa_grouping { get; set; }
        public int? typeofwork_grouping { get; set; }
        public int? status { get; set; }
        public bool? onlineSource { get; set; }
        public bool showPending { get; set; }
        public bool showOrdersPending { get; set; }
        public bool showOrdersWorkers { get; set; }
        public bool showInactiveWorker { get; set; }
        public bool showSanctionedWorker { get; set; }
        public bool showExpiredWorker { get; set; }
        public bool showExpelledWorker { get; set; }
        public bool attendedActivities { get; set; }

        // new for "People" tab
        public bool showExpiredWorkers { get; set; }
        public bool showSExWorkers { get; set; }
        public bool showNotWorkers { get; set; }
        public bool showWorkers { get; set; }

        #endregion
        /// <summary>
        /// Converts iSortCol_0 to an mDataProp name
        /// </summary>
        /// <returns></returns>
        public string sortColName()         
        {
            int idx = this.iSortCol_0;
            if (idx == 0) return this.mDataProp_0;
            if (idx == 1) return this.mDataProp_1;
            if (idx == 2) return this.mDataProp_2;
            if (idx == 3) return this.mDataProp_3;
            if (idx == 4) return this.mDataProp_4;
            if (idx == 5) return this.mDataProp_5;
            if (idx == 6) return this.mDataProp_6;
            if (idx == 7) return this.mDataProp_7;
            if (idx == 8) return this.mDataProp_8;
            if (idx == 9) return this.mDataProp_9;
            if (idx == 10) return this.mDataProp_10;
            if (idx == 11) return this.mDataProp_11;
            if (idx == 12) return this.mDataProp_12;
            if (idx == 13) return this.mDataProp_13;
            if (idx == 14) return this.mDataProp_14;
            if (idx == 15) return this.mDataProp_15;
            return null;
        }

        public string searchColName(string name)
        {
            if (this.mDataProp_0 == name) return this.sSearch_0;
            if (this.mDataProp_1 == name) return this.sSearch_1;
            if (this.mDataProp_2 == name) return this.sSearch_2;
            if (this.mDataProp_3 == name) return this.sSearch_3;
            if (this.mDataProp_4 == name) return this.sSearch_4;
            if (this.mDataProp_5 == name) return this.sSearch_5;
            if (this.mDataProp_6 == name) return this.sSearch_6;
            if (this.mDataProp_7 == name) return this.sSearch_7;
            if (this.mDataProp_8 == name) return this.sSearch_8;
            if (this.mDataProp_9 == name) return this.sSearch_9;
            if (this.mDataProp_10 == name) return this.sSearch_10;
            if (this.mDataProp_11 == name) return this.sSearch_11;
            if (this.mDataProp_12 == name) return this.sSearch_12;
            if (this.mDataProp_13 == name) return this.sSearch_13;
            if (this.mDataProp_14 == name) return this.sSearch_14;
            if (this.mDataProp_15 == name) return this.sSearch_15;
            return null;
        }
    }
}