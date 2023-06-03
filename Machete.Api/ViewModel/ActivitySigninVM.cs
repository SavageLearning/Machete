using Machete.Api.Helpers;

namespace Machete.Api.ViewModel
{
    public class ActivitySigninVM : SigninVM
    {
        public ActivitySigninVM() { }


        public virtual ActivityVM Activity { get; set; }
        public int activityID { get; set; }
        public int? personID { get; set; }
        public virtual PersonVM person { get; set; }
    }

    public class ActivitySigninListVM : ListVM
    {
        public int WSIID { get; set; }
        public string recordid { get; set; }
        public string fullname { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string dwccardnum { get; set; }
        public string dateforsignin { get; set; }
        public string memberStatus { get; set; }
        public bool memberInactive { get; set; }
        public bool memberSanctioned { get; set; }
        public bool memberExpired { get; set; }
        public bool memberExpelled { get; set; }
        public int? imageID { get; set; }
        public string expirationDate { get; set; }
    }
}