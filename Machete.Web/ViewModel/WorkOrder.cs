using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class WorkOrder : Domain.WorkOrder
    {
        public IDefaults def { get; set; }

        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string EID { get; set; }
        public string WOID { get; set; }

        public string recordid { get; set; }

        public string dateupdatedstring { get; set; }
        public string datecreatedstring { get; set; }
        //public string updatedby { get; set; }
        //public string createdby { get; set; }

        //public new string dateTimeofWork { get; set; }
        public new string disclosureAgreement { get; set; }
        public new string englishRequired { get; set; }
        public new string lunchSupplied { get; set; }
        public new string onlineSource { get; set; }
        public new string paperOrderNum { get; set; }
        public new string paypalFee { get; set; }
        public new string permanentPlacement { get; set; }
        //public new string status { get; set; }
        public new string transportFee { get; set; }
        public new string transportFeeExtra { get; set; }
        public new string transportMethodID { get; set; }
        public new string transportTransactType { get; set; }
        public new string typeOfWorkID { get; set; }
        public new string waPseudoIDCounter { get; set; }
    }

    public class WorkOrderList
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string EID { get; set; }
        public string WOID { get; set; }
        public string dateTimeofWork { get; set; }
        public string status { get; set; }
        public int statusID { get; set; }
        public string displayState { get; set; }
        public string transportMethod { get; set; }
        public string WAcount { get; set; }
        public string contactName { get; set; }
        public string workSiteAddress1 { get; set; }
        public string zipcode { get; set; }
        public string onlineSource { get; set; }
        public string emailSentCount { get; set; }
        public string emailErrorCount { get; set; }
        public string updatedby { get; set; }
        public string dateupdated { get; set; }
    }
}