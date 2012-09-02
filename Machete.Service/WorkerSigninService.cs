using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.Data.Objects;

namespace Machete.Service
{
    public interface IWorkerSigninService : ISigninService<WorkerSignin>
    {
        WorkerSignin GetSignin(int dwccardnum, DateTime date);
        int GetNextLotterySequence(DateTime date);
        bool clearLottery(int id, string user);
        bool sequenceLottery(DateTime date, string user);
        dataTableResult<wsiView> GetIndexView(viewOptions o);
        void CreateSignin(WorkerSignin signin, string user);
    }

    public class WorkerSigninService : SigninServiceBase<WorkerSignin>, IWorkerSigninService
    {
        //
        //
        public WorkerSigninService(IWorkerSigninRepository repo, 
                                   IWorkerRepository wRepo,
                                   IImageRepository iRepo,
                                   IWorkerRequestRepository wrRepo,
                                   IUnitOfWork uow)
            : base(repo, wRepo, iRepo, wrRepo, uow)
        {
            this.logPrefix = "WorkerSignin";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwccardnum"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public WorkerSignin GetSignin(int dwccardnum, DateTime date)
        {
            return repo.GetManyQ().FirstOrDefault(r => r.dwccardnum == dwccardnum &&
                            EntityFunctions.DiffDays(r.dateforsignin, date) == 0 ? true : false);
        }
        /// <summary>
        /// count lotteries for the day and increment
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int GetNextLotterySequence(DateTime date)
        {
            return repo.GetAllQ().Where(p => p.lottery_timestamp != null && 
                                        EntityFunctions.DiffDays(p.dateforsignin, date) == 0 ? true : false)
                                 .Count() + 1;
        }
        /// <summary>
        /// clear lottery timestamp and re-sequence
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool clearLottery(int id, string user)
        {
            WorkerSignin wsi = repo.GetById(id);
            DateTime date = wsi.dateforsignin;
            wsi.lottery_sequence = null;
            wsi.lottery_timestamp = null;
            Save(wsi, user);
            if (date != null)
                sequenceLottery(date, user);
            return true;
        }
        /// <summary>
        /// reset the ordinal lottery numbers
        /// </summary>
        /// <param name="date">date to reset numbers</param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool sequenceLottery(DateTime date, string user)
        {
            IEnumerable<WorkerSignin> signins;
            int i = 0;
            // select and sort by timestamp
            signins = repo.GetManyQ().Where(p => p.lottery_timestamp != null &&
                                           EntityFunctions.DiffDays(p.dateforsignin, date) == 0 ? true : false)
                                    .OrderBy(p => p.lottery_timestamp)
                                    .AsEnumerable();
            // reset sequence number
            foreach (WorkerSignin wsi in signins)
            {
                i++;
                wsi.lottery_sequence = i;                
            }
            // no logging as of now
            uow.Commit();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public dataTableResult<wsiView> GetIndexView(viewOptions o)
        {
            //
            var result = new dataTableResult<wsiView>();
            IQueryable<WorkerSignin> q = repo.GetAllQ();
            IEnumerable<WorkerSignin> e;
            IEnumerable<wsiView> eSIV;
            //
            if (o.date != null) IndexViewBase.diffDays(o, ref q);                
            //
            if (o.typeofwork_grouping == Worker.iDWC || o.typeofwork_grouping == Worker.iHHH)
                IndexViewBase.typeOfWork(o, ref q);
            // 
            // wa_grouping
            IndexViewBase.waGrouping(o, ref q, wrRepo);

            e = q.ToList();
            if (!string.IsNullOrEmpty(o.search))
                IndexViewBase.search(o, ref e);

            eSIV = e.Join(WorkerCache.getCache(), 
                            s => s.dwccardnum, 
                            w => w.dwccardnum, 
                            (s, w) => new { s, w }
                            )
                    .Select(z => new wsiView( z.w.Person, z.s ));

            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref eSIV);
            result.filteredCount = eSIV.Count();
            result.totalCount = repo.GetAllQ().Count();
            if ((int)o.displayLength >= 0)
                result.query = eSIV.Skip((int)o.displayStart).Take((int)o.displayLength);
            else
                result.query = eSIV;
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signin"></param>
        /// <param name="user"></param>
        public virtual void CreateSignin(WorkerSignin signin, string user)
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
            sfound = repo.GetAllQ().Count(s => s.dateforsignin == signin.dateforsignin &&
                                                     s.dwccardnum == signin.dwccardnum);
            if (sfound == 0) Create(signin, user);
        }
    }
}
