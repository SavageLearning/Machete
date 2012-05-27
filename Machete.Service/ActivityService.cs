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
    public interface IActivityService : IService<Activity>
    {
        IEnumerable<Activity> GetIndexView(viewOptions o);
    }
    /// <summary>
    /// 
    /// </summary>
    public class ActivityService : ServiceBase<Activity>, IActivityService
    {
        private IActivitySigninRepository asRepo;
        public ActivityService(IActivityRepository repo, 
            IActivitySigninRepository asRepo,
            IUnitOfWork uow) : base(repo, uow)
        {
            this.logPrefix = "Activity";
            this.asRepo = asRepo;
        }
        public IEnumerable<Activity> GetIndexView(viewOptions o)
        {
            IQueryable<Activity> q = repo.GetAllQ();
            IEnumerable<Activity> e;

            if (o.personID > 0) IndexViewBase.getUnassociated(o.personID, ref q, asRepo);
            e = q.AsEnumerable();
            if (!string.IsNullOrEmpty(o.search))
                IndexViewBase.search(o, ref e);

            IndexViewBase.sortOnColName(o.sortColName, 
                                        o.orderDescending, 
                                        o.CI.TwoLetterISOLanguageName, 
                                        ref e);
            //Limit results to the display length and offset
            return e.Skip(o.displayStart).Take(o.displayLength);
        }
    }
   
}
