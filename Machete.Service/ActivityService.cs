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
        dataTableResult<Activity> GetIndexView(viewOptions o);
        void AssignList(int personID, List<int> actList, string user);
        void UnassignList(int personID, List<int> actList, string user);
    }
    /// <summary>
    /// 
    /// </summary>
    public class ActivityService : ServiceBase<Activity>, IActivityService
    {
        private IActivitySigninService asServ;
        public ActivityService(IActivityRepository repo,
            IActivitySigninService asServ,
            IUnitOfWork uow) : base(repo, uow)
        {
            this.logPrefix = "Activity";
            this.asServ = asServ;
        }
        public dataTableResult<Activity> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<Activity>();
            IQueryable<Activity> q = repo.GetAllQ();
            IEnumerable<Activity> e;
            var asRepo = (IActivitySigninRepository)asServ.GetRepo();

            if (o.personID > 0 && o.attendedActivities == false)
                IndexViewBase.getUnassociated(o.personID, ref q, repo, asRepo);
            if (o.personID > 0 && o.attendedActivities == true)
                IndexViewBase.getAssociated(o.personID, ref q, asRepo);

            e = q.AsEnumerable();
            if (!string.IsNullOrEmpty(o.sSearch))
                IndexViewBase.search(o, ref e);

            IndexViewBase.sortOnColName(o.sortColName, 
                                        o.orderDescending, 
                                        o.CI.TwoLetterISOLanguageName, 
                                        ref e);
            //Limit results to the display length and offset
            //if (o.displayLength >= 0)
            result.filteredCount = e.Count();
            result.totalCount = repo.GetAllQ().Count();
            result.query = e.Skip(o.displayStart).Take(o.displayLength);
            return result;
        }

        public void AssignList(int personID, List<int> actList, string user)
        {
            foreach (int aID in actList)
            {
                Activity act = repo.GetById(aID);
                if (act == null) throw new Exception("Activity from list is null");
                int matches = asServ.GetManyByPersonID(aID, personID).Count();

                if (matches == 0)
                {
                    asServ.CreateSignin(new ActivitySignin
                    {
                        activityID = aID,
                        personID = personID,
                        dateforsignin = act.dateStart
                    }, user);
                }
            }
        }

        public void UnassignList(int personID, List<int> actList, string user)
        {
            foreach (int aID in actList)
            {
                Activity act = repo.GetById(aID);
                if (act == null) throw new Exception("Activity from list is null");
                ActivitySignin asi = asServ.GetByPersonID(aID, personID);
                if (asi == null) throw new NullReferenceException("ActivitySignin.GetByPersonID returned null");
                asServ.Delete(asi.ID, user);
            }
        }
    }
   
}
