using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;
using System.Globalization;

namespace Machete.Domain
{
    public class WorkerSignin : Signin 
    {
        public int? WorkAssignmentID { get; set; }
        public DateTime? lottery_timestamp { get; set; }
        public int? lottery_sequence { get; set; }
    }
    public abstract class Signin : Record
    {
        //public int ID { get; set; }
        public virtual Worker worker {get; set;}
        [Required(ErrorMessageResourceName = "dwccardnum", ErrorMessageResourceType = typeof(Resources.Worker))]
        [RegularExpression("^[0-9]{5,5}$", ErrorMessageResourceName = "dwccardnumerror", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dwccardnum", NameResourceType = typeof(Resources.Worker))]
        public int dwccardnum { get; set; }
        public int? WorkerID { get; set; }
        public int? memberStatus { get; set; }
        public DateTime dateforsignin { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class signinView : Record
    {
        public int dwccardnum { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string fullname
        {
            get
            {
                return firstname1 + " " +
                        firstname2 + " " +
                        lastname1 + " " +
                        lastname2;
            }
            set{}
        }
        public int signinID { get; set; }
        public DateTime dateforsignin { get; set; }
        public int? imageID { get; set; }
        public DateTime expirationDate { get; set; }
        public int memberStatus { get; set; }
        public Person p { get; set; }
        public Worker w { get; set; }
        public Signin s { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="per"></param>
        /// <param name="sign"></param>
        public signinView(Person per, Signin sign)
        {
            p = per;
            w = p.Worker;
            s = sign;
            ID = s.ID;
            firstname1 = p == null ? null : p.firstname1;
            firstname2 = p == null ? null : p.firstname2;
            lastname1 = p == null ? null : p.lastname1;
            lastname2 = p == null ? null : p.lastname2;
            dateforsignin = s.dateforsignin;
            dwccardnum = s.dwccardnum;
            signinID = s.ID;
            dateupdated = s.dateupdated;
            datecreated = s.datecreated;
            Createdby = s.Createdby;
            Updatedby = s.Updatedby;
            imageID = p == null ? null : p.Worker.ImageID;
            expirationDate = p == null ? DateTime.MinValue : p.Worker.memberexpirationdate;
            memberStatus = p == null ? 0 : p.Worker.memberStatus;

        }
        public signinView() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class wsiView : signinView
    {
        public int? lotterySequence { get; set; }
        public int? skill1 { get; set; }
        public int? skill2 { get; set; }
        public int? skill3 { get; set; }
        public int? waid { get; set; }
        public int englishlevel { get; set; }
        public int typeOfWorkID { get; set; }
        public wsiView(Person per, WorkerSignin sign)
            : base(per, sign)
        {
            lotterySequence = sign.lottery_sequence;
            englishlevel = p == null ? 0 : p.Worker.englishlevelID;
            waid = sign.WorkAssignmentID;
            skill1 = p == null ? null : p.Worker.skill1;
            skill2 = p == null ? null : p.Worker.skill2;
            skill3 = p == null ? null : p.Worker.skill3;
            typeOfWorkID = p == null ? 0 : p.Worker.typeOfWorkID;
        }
    }
    public class asiView : signinView
    {
        public int type { get; set; }
        public string teacher { get; set; }
        public int name { get; set; }
        public Activity a { get; set; }
        //public DateTime dateStart { get; set; }
        //public DateTime dateEnd { get; set; }
        public asiView(Person per, ActivitySignin sign)
            : base(per, sign)
        {
            type = sign.Activity.type;
            teacher = sign.Activity.teacher;
            name = sign.Activity.name;
            a = sign.Activity;
        }
    }

    //public class signinViewConverter : TypeConverter
    //{
    //    // TypeConverter extension
    //    // http://msdn.microsoft.com/en-us/library/ayybcxe5.aspx
    //    public override bool CanConvertFrom(ITypeDescriptorContext context,Type sourceType)
    //    {
    //        if (sourceType == typeof(signinView))
    //        {
    //            return true;
    //        }
    //        return base.CanConvertFrom(context, sourceType);
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context,
    //            CultureInfo culture, object value)
    //    {
    //        if (value is signinView)
    //        {
    //            //string[] v = ((string)value).Split(new char[] { ',' });
    //            //return new Point(int.Parse(v[0]), int.Parse(v[1]));
    //            return new asiView(((signinView)value).p, ((signinView)value).s);

    //        }
    //        return base.ConvertFrom(context, culture, value);
    //    }

    //    public override object ConvertTo(ITypeDescriptorContext context,
    //       CultureInfo culture, object value, Type destinationType)
    //    {
    //        if (destinationType == typeof(asiView))
    //        {
    //            return new signinView(((asiView)value).p, ((asiView)value).s);
    //        }
    //        return base.ConvertTo(context, culture, value, destinationType);
    //    }
    //}
}
