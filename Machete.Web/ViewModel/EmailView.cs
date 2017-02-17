using Machete.Domain;
using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Machete.Web.ViewModel
{
    public class EmailView : Record
    {
        [LocalizedDisplayName("emailFrom", NameResourceType = typeof(Resources.Emails))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Emails))]
        public string emailFrom { get; set; }
        //
        [LocalizedDisplayName("emailTo", NameResourceType = typeof(Resources.Emails))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Emails))]
        [Required(ErrorMessageResourceName = "emailTo", ErrorMessageResourceType = typeof(Resources.Emails))]
        public string emailTo { get; set; }
        //
        [LocalizedDisplayName("subject", NameResourceType = typeof(Resources.Emails))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Emails))]
        [Required(ErrorMessageResourceName = "subject", ErrorMessageResourceType = typeof(Resources.Emails))]
        public string subject { get; set; }
        //
        [LocalizedDisplayName("body", NameResourceType = typeof(Resources.Emails))]
        [StringLength(8000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Emails))]
        [Required(ErrorMessageResourceName = "body", ErrorMessageResourceType = typeof(Resources.Emails))]
        public string body { get; set; }

        public int transmitAttempts { get; set; }
        public string status { get; set; }
        public string attachment { get; set; }
        public string attachmentContentType { get; set; }
        public int statusID { get; set; }
        public DateTime? lastAttempt { get; set; }
        public int? woid { get; set; }
        public IDefaults def { get; set;  }
        //
        // view-only fields
        //
        public List<SelectListItemEmail> templates { get; set; }
        public EmailView() { }
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