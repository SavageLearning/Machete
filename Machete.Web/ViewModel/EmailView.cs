using Machete.Domain;
using Machete.Web.Helpers;
using Machete.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
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
        public int statusID { get; set; }
        public DateTime? lastAttempt { get; set; }
        //
        // view-only fields
        //
        public string recType { get; set; }
        public string idPrefix { get; set; }

        public List<SelectListItemEmail> templates { get; set; }
        public SelectList statusCategories { get; set; }

        public EmailView(Email email, string locale)
        {
            status = Lookups.byID(email.statusID, locale);
            templates = Lookups.getEmailTemplates(locale);
            statusCategories = Lookups.get(Machete.Domain.LCategory.emailstatus, locale);
            recType = "email";
            idPrefix = recType + ID + "-";
        }

        public EmailView(string locale)
        {
        }

        public bool editable
        {
            get
            {
                if (statusID == Email.iPending
                    || statusID == Email.iReadyToSend
                    || statusID == Email.iTransmitError)
                {
                    return true;
                }
                return false;
            }
        }
    }
}