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
        void CreateSignin(WorkerSignin workerSignin, string user);
        int GetNextLotterySequence(DateTime date);
        bool clearLottery(int id, string user);
        bool sequenceLottery(DateTime date, string user);
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
    }
}
