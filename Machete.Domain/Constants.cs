using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
// The constants here are strings that are used both in the database and the application
// as identifiers of system crtical values. These structs encapsulate key strings that machete
// depends on for proper functionality.
//
// The ID values in the Lookup table may vary from installation to installation, these values should be
// the same. These values are used in the lookup table's category and key columns. Machete may behave in 
// unexpected ways if you modify these. 
namespace Machete.Domain
{
    public struct LCategory
    {
        public const string race = "race";
        public const string language = "language";
        public const string neighborhood = "neighborhood";
        public const string income = "income";
        public const string worktype = "worktype";
        public const string emplrreference = "emplrreference";
        public const string transportmethod = "transportmethod";
        public const string maritalstatus = "maritalstatus";
        public const string gender = "gender";
        public const string orderstatus = "orderstatus";
        public const string countryoforigin = "countryoforigin";
        public const string skill = "skill";
        public const string eventtype = "eventtype";
        public const string memberstatus = "memberstatus";
        public const string activityName = "activityName";
        public const string activityType = "activityType";
        public const string emailstatus = "emailstatus";
    }

    public struct LMemberStatus
    {
        public const string Active= "Active";
        public const string Sanctioned= "Sanctioned";
        public const string Expelled= "Expelled";
        public const string Expired= "Expired";
        public const string Inactive = "Inactive";
    }

    public struct LOrderStatus
    {
        public const string Active= "Active";
        public const string Pending= "Pending";
        public const string Completed= "Completed";
        public const string Cancelled= "Cancelled";
        public const string Expired = "Expired";
    }
    public struct LEmailStatus
    {
        public const string ReadyToSend = "readytosend";
        public const string Sent = "sent";
        public const string TransmitError = "transmiterror";
    }
    public struct LKey
    {
        public const string Default = "default";
    }

}
