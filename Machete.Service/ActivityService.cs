using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Data.Objects;

namespace Machete.Service
{
    public interface IActivityService : IService<Activity>
    {
        dTableList<Activity> GetIndexView(viewOptions o);
    }

    public class ActivityService : ServiceBase<Activity>, IActivityService
    {
        private ILookupRepository lRepo;
        public ActivityService(IActivityRepository repo, 
            ILookupRepository lRepo,
            IUnitOfWork uow) : base(repo, uow)
        {
            this.logPrefix = "Activity";
            this.lRepo = lRepo;
        }
        public dTableList<Activity> GetIndexView(viewOptions o)
        {
            IEnumerable<Activity> q = repo.GetAll();
            if (!string.IsNullOrEmpty(o.search))
                IndexViewBase.search(o, ref q, lRepo);


            IndexViewBase.sortOnColName(o.sortColName, 
                                        o.orderDescending, 
                                        o.CI.TwoLetterISOLanguageName, 
                                        ref q);
            //Limit results to the display length and offset
            q = q.Skip(o.displayStart).Take(o.displayLength);
            return new dTableList<Activity>
            {
                query = q,
                filteredCount = q.Count(),
                totalCount = repo.GetAllQ().Count()
            };

        }
    }

    public interface IActivitySigninService : ISigninService<ActivitySignin>
    {
        void CreateSignin(ActivitySignin signin, string user);
    }

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
        public override dTableList<wsiView> GetIndexView(viewOptions o)
        {
            IQueryable<ActivitySignin> queryable = repo.GetAllQ();
            //
            // WHERE on ActivityID
             if (o.ActivityID != null)
                 queryable = queryable.Where(p => p.ActivityID == o.ActivityID);
            //
             return base.GetIndexView(o, queryable);
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
