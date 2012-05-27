using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Data.Objects;
using System.ComponentModel;

namespace Machete.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActivitySigninService : ISigninService<ActivitySignin>
    {
        IEnumerable<asiView> GetIndexView(viewOptions o);
    }
    /// <summary>
    /// 
    /// </summary>
    public class ActivitySigninService : SigninServiceBase<ActivitySignin>, IActivitySigninService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="wRepo"></param>
        /// <param name="pRepo"></param>
        /// <param name="iRepo"></param>
        /// <param name="wrRepo"></param>
        /// <param name="uow"></param>
        public ActivitySigninService(IActivitySigninRepository repo,
                                   IWorkerRepository wRepo,
                                   IPersonRepository pRepo,
                                   IImageRepository iRepo,
                                   IWorkerRequestRepository wrRepo,
                                   IUnitOfWork uow)
            : base(repo, wRepo, iRepo, wrRepo, uow)
        {
            this.logPrefix = "ActivitySignin";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public IEnumerable<asiView> GetIndexView(viewOptions o)
        {
            IQueryable<ActivitySignin> q = repo.GetAllQ();
            IEnumerable<ActivitySignin> e;
            IEnumerable<asiView> eSIV;
            //
            if (o.date != null) IndexViewBase.diffDays(o, ref q);
            // WHERE on ActivityID
            if (o.personID > 0)
                IndexViewBase.GetAssociated(o.personID, ref q);
            if (o.ActivityID != null)
                q = q.Where(p => p.ActivityID == o.ActivityID);
            //            
            e = q.ToList();
            if (!string.IsNullOrEmpty(o.search))
                IndexViewBase.search(o, ref e);

            eSIV = e.Join(WorkerCache.getCache(),
                            s => s.dwccardnum,
                            w => w.dwccardnum,
                            (s, w) => new { s, w }
                            )
                    .Select(z => new asiView(z.w.Person, z.s));

            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, o.CI.TwoLetterISOLanguageName, ref eSIV);
            //if ((int)o.displayLength >= 0)
            return eSIV.Skip((int)o.displayStart).Take((int)o.displayLength);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signin"></param>
        /// <param name="user"></param>
        public override void CreateSignin(ActivitySignin signin, string user)
        {
            //Search for worker with matching card number
            Worker wfound;
            wfound = wRepo.GetAllQ().FirstOrDefault(s => s.dwccardnum == signin.dwccardnum);
            if (wfound != null)
            {
                signin.WorkerID = wfound.ID;
            }
            //Search for duplicate signin for the same day
            int sfound = 0; ;
            sfound = repo.GetAllQ().Count(s => //s.dateforsignin == signin.dateforsignin &&
                                                     s.ActivityID == signin.ActivityID &&
                                                     s.dwccardnum == signin.dwccardnum);
            if (sfound == 0) Create(signin, user);
        }
    }
}
