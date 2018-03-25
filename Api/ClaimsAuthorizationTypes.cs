using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api
{
    public struct CAType
    {
        // used in Lookups.key ; max length 30 characters
        public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public const string nameidentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public const string email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    }

    public struct CV
    {
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
        public const string PaypalId = "PayPalClientID";
        public const string PaypalSecret = "PayPalClientSecret";
        public const string PaypalUrl = "PayPalUrl";
    }
}