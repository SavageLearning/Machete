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
    }

    public class ActivityService : ServiceBase<Activity>, IActivityService
    {
        public ActivityService(IActivityRepository repo, IUnitOfWork uow) : base(repo, uow)
        {
            this.logPrefix = "Activity";
        }
    }

    public interface IActivitySigninService : ISigninService<ActivitySignin>
    {
        void CreateSignin(ActivitySignin workerSignin, string user);
    }

    public class ActivitySigninService : SigninServiceBase<ActivitySignin>, IActivitySigninService
    {
        //
        //
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
        public override ServiceIndexView<WorkerSigninView> GetIndexView(dispatchViewOptions o)
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
