//
// The constants here are strings that are used both in the database and the application
// as identifiers of system crtical values. These structs encapsulate key strings that machete
// depends on for proper functionality.
//
// The ID values in the Lookup table may vary from installation to installation, these values should be
// the same. These values are used in the lookup table's category and key columns. Machete may behave in 
// unexpected ways if you modify these. 
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        public const string OnlineOrdersIntroMessage = "OnlineOrdersIntroMessage";
        public const string OnlineOrdersTerms = "OnlineOrdersTerms";
        public const string OnlineOrdersEnglishReqNote = "OnlineOrdersEnglishReqNote";
        public const string OnlineOrdersTransportDetailsLink = "OnlineOrdersTransportDetailsLink";
        public const string OrderConfirmTransportFeesNotice = "OrderConfirmTransportFeesNotice";
        public const string OrderReviewWorkerCountLabel = "OrderReviewWorkerCountLabel";
        public const string OrderReviewSkillsSummaryLabel = "OrderReviewSkillsSummaryLabel";
        public const string OrderReviewTransportFeeMethodHelper = "OrderReviewTransportFeeMethodHelper";
        public const string OrderReviewLaborCostMethodHelper = "OrderReviewLaborCostMethodHelper";
        public const string OrderReviewTransportFeeAboutUrl = "OrderReviewTransportFeeAboutUrl";
        public const string OrderReviewLaborCostAboutUrl = "OrderReviewLaborCostAboutUrl";
        public const string OrderReviewNextStepsHelper = "OrderReviewNextStepsHelper";
        public const string DisableWorkersVaccineRequirement = "DisableWorkersVaccineRequirement";
        public const string MicrosoftTimeZoneIndex = "MicrosoftTimeZoneIndex";
        public const string DisableOnlineOrders = "DisableOnlineOrders";
        public const string DisableOnlineOrdersBanner = "DisableOnlineOrdersBanner";
        public const string DisableOnlineOrdersBannerInfoUrl = "DisableOnlineOrdersBannerInfoUrl";
    }

    public static class UserDefinedConfigs
    {
        private static readonly List<string> _userDefinedConfigs = new List<string>()
        {
            Cfg.OrganizationName,
            Cfg.OrganizationAddress,
            Cfg.WorkCenterDescription,
            Cfg.OnlineOrdersIntroMessage,
            Cfg.OnlineOrdersTerms,
            Cfg.OnlineOrdersEnglishReqNote,
            Cfg.OnlineOrdersTransportDetailsLink,
            Cfg.OrderConfirmTransportFeesNotice,
            Cfg.OrderReviewWorkerCountLabel,
            Cfg.OrderReviewSkillsSummaryLabel,
            Cfg.OrderReviewTransportFeeMethodHelper,
            Cfg.OrderReviewLaborCostMethodHelper,
            Cfg.OrderReviewTransportFeeAboutUrl,
            Cfg.OrderReviewLaborCostAboutUrl,
            Cfg.OrderReviewNextStepsHelper,
            Cfg.DisableWorkersVaccineRequirement,
            Cfg.DisableOnlineOrders,
            Cfg.DisableOnlineOrdersBanner,
            Cfg.DisableOnlineOrdersBannerInfoUrl
        };

        public static ReadOnlyCollection<string> Pascal = _userDefinedConfigs.AsReadOnly();
        public static ReadOnlyCollection<string> Lower = _userDefinedConfigs
                                                            .ConvertAll(item => item.ToLower())
                                                            .AsReadOnly();
    }
}
