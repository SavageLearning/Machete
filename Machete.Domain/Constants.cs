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
        // used in Lookups.key ; max length 30 characters
        public const string race = "race";
        public const string language = "language";
        public const string neighborhood = "neighborhood";
        public const string income = "income";
        public const string worktype = "worktype";
        public const string emplrreference = "emplrreference";
        public const string transportmethod = "transportmethod";
        public const string transportTransactType = "transportPaymentType";
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
        public const string emailTemplate = "emailTemplate";
        public const string workerRating = "workerRating";
        public const string housingType = "housingType";
        public const string educationLevel = "educationLevel";
        public const string farmLabor = "farmLabor";
        public const string vehicleTypeID = "vehicleTypeID";
        public const string incomeSourceID = "incomeSourceID";
        public const string usBornChildren = "usBornChildren";
        public const string training = "training";
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
        // Adding orphaned and unassigned here, but not in Lookups
        // These status options shouldn't be selectable by the user; they're 
        // the result of an expression that detects incomplete records
        public const string Orphaned = "Orphaned";
        public const string Unassigned = "Unassigned";
    }
    public struct LEmailStatus
    {
        public const string Pending = "pending";
        public const string ReadyToSend = "readytosend";
        public const string Sending = "sending";
        public const string Sent = "sent";
        public const string TransmitError = "transmiterror";
    }

    public struct LKey
    {
        public const string Default = "default";
        public const string Skill = "skill";
    }
    public struct LWorkType
    {
        public const string DWC = "DWC";
        public const string HHH = "HHH";
        public const string EVT = "EVT";
    }
    public struct LActType
    {
        public const string Class = "Class";
        public const string Workshop = "Workshop";
        public const string Assembly = "Assembly";
        public const string OrgMtg = "Organizing Meeting";
    }
    public struct LActName
    {
        public const string Assembly = "Assembly";
        public const string OrgMtg = "Organizing Meeting";
    }

    public struct CV
    {
        public const string Any = "Any";
        public const string Admin = "Administrator";
        public const string Employer = "Hirer";
        public const string User = "User";
        public const string Phonedesk = "Phonedesk";
        public const string Manager = "Manager";
        public const string Checkin = "Check-in";
        public const string Teacher = "Teacher";
    }

    public struct Cfg
    {
        public const string OrganizationName = "OrganizationName";
        public const string OrganizationAddress = "OrganizationAddress"; // used on printed order form
        public const string PaypalId = "PayPalClientID";
        public const string PaypalSecret = "PayPalClientSecret";
        public const string PaypalUrl = "PayPalUrl";
        public const string PaypalEnvironment = "PayPalEnvironment";
        public const string EmailServerHostName = "EmailServerHostName";
        public const string EmailServerPort = "EmailServerPort";
        public const string EmailEnableSSL = "EmailEnableSSL";
        public const string SmtpUser = "SmtpUser";
        public const string SmtpPassword = "SmtpPassword";
        public const string EmailFromAddress = "EmailFromAddress";
        public const string EmailEnableSimple = "true";
        public const string TimeZoneDifferenceFromPacific = "TimeZoneDifferenceFromPacific";
        public const string WorkCenterDescription = "WorkCenterDescription_EN";
    }
}