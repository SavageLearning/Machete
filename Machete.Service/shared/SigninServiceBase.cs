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
    public interface ISigninService<T> : IService<T> where T : Signin
    {
        //T GetSignin(int dwccardnum, DateTime date);
        void CreateSignin(T workerSignin, string user);
        Image getImage(int dwccardnum);
        ServiceIndexView<WorkerSigninView> GetIndexView(DispatchOptions o);
        //ServiceIndexView<WorkerSigninView> GetIndexView(DispatchOptions o, IQueryable<T> repo);
    }
    public abstract class SigninServiceBase<T> : ServiceBase<T> where T : Signin
    {
        protected readonly IWorkerRepository wRepo;
        protected readonly IWorkerRequestRepository wrRepo;
        protected readonly IPersonRepository pRepo;
        protected readonly IImageRepository iRepo;
        //
        //
        protected SigninServiceBase(IRepository<T> repo,
                                   IWorkerRepository wRepo,
                                   IPersonRepository pRepo,
                                   IImageRepository iRepo,
                                   IWorkerRequestRepository wrRepo,
                                   IUnitOfWork uow)
            : base(repo, uow)
        {
            this.wRepo = wRepo;
            this.pRepo = pRepo;
            this.wrRepo = wrRepo;
            this.iRepo = iRepo;
            this.logPrefix = "SigninServiceBase";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signin"></param>
        /// <param name="user"></param>
        public void CreateSignin(T signin, string user)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public ServiceIndexView<WorkerSigninView> GetIndexView(DispatchOptions o)
        {
            return GetIndexView(o, repo.GetAllQ());
        }
        protected ServiceIndexView<WorkerSigninView> GetIndexView(DispatchOptions o, IQueryable<Signin> queryable)
        {
            //Get all the records
            IQueryable<Signin> queryableWSI = queryable;
            IEnumerable<Signin> enumWSI;
            IEnumerable<WorkerSigninView> enumWSIV;
            //Search based on search-bar string
            //DateTime parsedTime;
            if (o.date != null)
                queryableWSI = queryableWSI.Where(p => EntityFunctions.DiffDays(p.dateforsignin, o.date) == 0 ? true : false);
            // 
            // typeofwork ( DWC / HHH )
            //TODO: Remove Casa specific configuration. needs real abstraction on iDWC / iHHH.
            if (o.typeofwork_grouping == Worker.iDWC)
                queryableWSI = queryableWSI
                                         .Where(wsi => wsi.worker.typeOfWorkID == Worker.iDWC)
                                         .Select(wsi => wsi);

            if (o.typeofwork_grouping == Worker.iHHH)
                queryableWSI = queryableWSI
                                         .Where(wsi => wsi.worker.typeOfWorkID == Worker.iHHH)
                                         .Select(wsi => wsi);
            // 
            // wa_grouping
            switch (o.wa_grouping)
            {
                case "open": queryableWSI = queryableWSI.Where(p => p.WorkAssignmentID == null); break;
                case "assigned": queryableWSI = queryableWSI.Where(p => p.WorkAssignmentID != null); break;
                case "skilled": queryableWSI = queryableWSI
                                     .Where(wsi => wsi.WorkAssignmentID == null &&
                                         wsi.worker.skill1 != null ||
                                         wsi.worker.skill2 != null ||
                                         wsi.worker.skill3 != null
                    );

                    break;
                case "requested":
                    if (o.date == null) throw new MacheteIntegrityException("Date cannot be null for Requested filter");
                    queryableWSI = queryableWSI.Where(p => p.WorkAssignmentID == null);
                    queryableWSI = queryableWSI.Join(wrRepo.GetAllQ(),
                                                wsi => new { K1 = (int)wsi.WorkerID, K2 = (DateTime)EntityFunctions.TruncateTime(wsi.dateforsignin) },
                                                wr => new { K1 = wr.WorkerID, K2 = (DateTime)EntityFunctions.TruncateTime(wr.workOrder.dateTimeofWork) },
                                                (wsi, wr) => wsi);
                    break;
            }
            if (!string.IsNullOrEmpty(o.search))
            {
                enumWSI = queryableWSI.ToList()
                            .Join(WorkerCache.getCache(), s => s.dwccardnum, w => w.dwccardnum, (s, w) => new { s, w })
                            .Where(p => p.w.dwccardnum.ToString().ContainsOIC(o.search) ||
                                        p.w.Person.firstname1.ContainsOIC(o.search) ||
                                        p.w.Person.firstname2.ContainsOIC(o.search) ||
                                        p.w.Person.lastname1.ContainsOIC(o.search) ||
                                        p.w.Person.lastname2.ContainsOIC(o.search))
                            .Select(a => a.s);
            }
            else
            {
                enumWSI = queryableWSI.ToList();
            }
            enumWSIV = enumWSI
                    .Join(WorkerCache.getCache(), s => s.dwccardnum, w => w.dwccardnum, (s, w) => new { s, w })
                    .Select(p => new WorkerSigninView(p.w.Person, p.s));

            //Sort the Persons based on column selection
            switch (o.sortColName)
            {
                case "dwccardnum": enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.dwccardnum) : enumWSIV.OrderBy(p => p.dwccardnum); break;
                case "firstname1": enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.firstname1) : enumWSIV.OrderBy(p => p.firstname1); break;
                case "firstname2": enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.firstname2) : enumWSIV.OrderBy(p => p.firstname2); break;
                case "lastname1": enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.lastname1) : enumWSIV.OrderBy(p => p.lastname1); break;
                case "lastname2": enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.lastname2) : enumWSIV.OrderBy(p => p.lastname2); break;
                case "lotterySequence": enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.lotterySequence != null).ThenByDescending(p => p.lotterySequence) : 
                        enumWSIV.OrderBy(p => p.lotterySequence == null).ThenBy(p => p.lotterySequence); break;
                case "dateupdated": enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.dateupdated) : enumWSIV.OrderBy(p => p.dateupdated); break;
                case "dateforsigninstring": enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.dateforsignin) : enumWSIV.OrderBy(p => p.dateforsignin); break;
                case "expirationDate": enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.expirationDate) : enumWSIV.OrderBy(p => p.expirationDate); break;
                default: enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.dateforsignin) : enumWSIV.OrderBy(p => p.dateforsignin); break;
            }
            //queryableWSI = queryableWSI.ToList();
            var filtered = enumWSIV.Count();
            if ((int)o.displayLength >= 0)
                enumWSIV = enumWSIV.Skip<WorkerSigninView>((int)o.displayStart).Take((int)o.displayLength);

            var total = repo.GetAllQ().Count();

            return new ServiceIndexView<WorkerSigninView>
            {
                query = enumWSIV,
                filteredCount = filtered,
                totalCount = total
            };

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardrequest"></param>
        /// <returns></returns>
        public Image getImage(int cardrequest)
        {
            Worker w_query = wRepo.GetAllQ().Where(w => w.dwccardnum == cardrequest).AsEnumerable().FirstOrDefault();
            if (w_query == null) return null;
            if (w_query.ImageID != null)
            {
                return iRepo.Get(i => i.ID == w_query.ImageID);
            }
            return null;
        } 
    }
}