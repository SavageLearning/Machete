using Machete.Domain;
using Machete.Api.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Machete.Api.ViewModel
{
    public class EmailViewVM : RecordVM
    {
        [StringLength(50)]
        public string emailFrom { get; set; }
        //
        [StringLength(50)]
        [Required]
        public string emailTo { get; set; }
        //
        [StringLength(100)]
        [Required]
        public string subject { get; set; }
        //
        [StringLength(8000)]
        [Required]
        public string body { get; set; }

        public int transmitAttempts { get; set; }
        public string status { get; set; }
        public string attachment { get; set; }
        public string attachmentContentType { get; set; }
        public int statusID { get; set; }
        public DateTime? lastAttempt { get; set; }
        public int? woid { get; set; }
        //
        // view-only fields
        //
        public EmailViewVM() { }
        public bool editable
        {
            get
            {
                if (statusID == Domain.Email.iPending
                    || statusID == Domain.Email.iReadyToSend
                    || statusID == Domain.Email.iTransmitError)
                {
                    return true;
                }
                return false;
            }
        }
    }
}