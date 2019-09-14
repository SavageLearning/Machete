using Machete.Web.Helpers;

namespace Machete.Web.ViewModel
{
    public class ActivitySignin : Signin
    {
        public ActivitySignin()
        {
            idString = "asi";
        }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }

        public virtual Activity Activity { get; set; }
        public int activityID { get; set; }
        public int? personID { get; set; }
        public virtual Person person { get; set; }
    }

    public class ActivitySigninList
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
    //var result = from p in was.query
    //             select new
    //             {
    //                 WSIID = p.ID,
    //                 recordid = p.ID.ToString(),
    //                 dwccardnum = p.dwccardnum,
    //                 fullname = p.fullname,
    //                 firstname1 = p.firstname1,
    //                 firstname2 = p.firstname2,
    //                 lastname1 = p.lastname1,
    //                 lastname2 = p.lastname2,
    //                 dateforsignin = p.dateforsignin,
    //                 dateforsigninstring = p.dateforsignin.ToShortDateString(),
    //                 memberStatus = lcache.textByID(p.memberStatus, currentCulture.TwoLetterISOLanguageName),
    //                 memberInactive = p.w.isInactive,
    //                 memberSanctioned = p.w.isSanctioned,
    //                 memberExpired = p.w.isExpired,
    //                 memberExpelled = p.w.isExpelled,
    //                 imageID = p.imageID,
    //                 expirationDate = p.expirationDate.ToShortDateString(),
    //             };
}