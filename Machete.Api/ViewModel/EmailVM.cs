using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Machete.Domain;

namespace Machete.Api.ViewModel
{
    public class EmailVM : RecordVM
    {
        public static int iPending { get; set; }
        public static int iReadyToSend { get; set; }
        public static int iSending { get; set; }
        public static int iSent { get; set; }
        public static int iTransmitError { get; set; }
        public static int iTransmitAttempts { get; set; }
        public virtual ICollection<WorkOrderVM> WorkOrders { get; set; }
        [StringLength(50)]
        public string emailFrom { get; set; }
        [StringLength(50), Required()]
        public string emailTo { get; set; }
        [StringLength(100), Required()]
        public string subject { get; set; }
        [StringLength(8000), Required()]
        public string body { get; set; }
        public int transmitAttempts { get; set; }
        public int statusID { get; set; }
        public DateTime? lastAttempt { get; set; }
        public string attachment { get; set; }
        public string attachmentContentType { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public bool isAssociatedToWorkOrder
        {
            get
            {
                if (this.WorkOrders.Count() > 0) return true;
                return false;
            }
        }
    }

    public class EmailListVM : ListVM
    {

    }
}