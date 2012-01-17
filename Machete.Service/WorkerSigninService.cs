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
    public interface IWorkerSigninService
    {
        IQueryable<WorkerSignin> GetWorkerSigninsQ();
        WorkerSignin GetWorkerSignin(int id);
        WorkerSignin GetWorkerSignin(int dwccardnum, DateTime date);
        void CreateWorkerSignin(WorkerSignin workerSignin, string user);
        void DeleteWorkerSignin(int id);
        void SaveWorkerSignin();
        IEnumerable<WorkerSigninView> getView(DateTime date);
        Image getImage(int dwccardnum);
        DateTime getExpireDate(int dwccardnum);
        ServiceIndexView<WorkerSigninView> GetIndexView(DispatchOptions o);
        IEnumerable<WorkerSignin> GetSigninsForAssignment(DateTime date, string search, string order, int? displayStart, int? displayLength);
    }
    public class WorkerSigninService : IWorkerSigninService
    {
        private readonly IWorkerSigninRepository signinRepo;
        private readonly IWorkerRepository workerRepo;
        private readonly IWorkerRequestRepository wrRepo;
        private readonly IPersonRepository personRepo;
        private readonly IUnitOfWork unitOfWork;
        //private readonly ILookupRepository lRepo;
        private readonly IImageRepository imageRepo;
        private static int lkup_dwc;
        private static int lkup_hhh;
        //
        //
        public WorkerSigninService(IWorkerSigninRepository workerSigninRepository, 
                                   IWorkerRepository workerRepository,
                                   IPersonRepository personRepository,
                                   IImageRepository imageRepository,
                                   IWorkerRequestRepository workerRequestRepository,
                                   //ILookupRepository lookupRepository,
                                   IUnitOfWork unitOfWork)
        {
            this.signinRepo = workerSigninRepository;
            this.workerRepo = workerRepository;
            this.personRepo = personRepository;
            this.wrRepo = workerRequestRepository;
            //this.lRepo = lookupRepository;
            this.unitOfWork = unitOfWork;
            this.imageRepo = imageRepository;
            lkup_dwc = LookupCache.getSingleEN("worktype", "(DWC) Day Worker Center");
            lkup_hhh = LookupCache.getSingleEN("worktype", "(HHH) Household Helpers");
        }
        #region IWorkerSigninService Members
        
        public IQueryable<WorkerSignin> GetWorkerSigninsQ()
        {
            return signinRepo.GetAllQ();
 
        }
        
        public WorkerSignin GetWorkerSignin(int id)
        {
            var workerSignin = signinRepo.GetById(id);
            return workerSignin;
        }

        public WorkerSignin GetWorkerSignin(int dwccardnum, DateTime date)
        {
            //.AsQueryable().FirstOrDefault(r => r.dwccardnum == 30040 && EntityFunctions.DiffDays(r.dateforsignin, datestr) == 0 ? true : false);
            var workerSignin = signinRepo.GetManyQ();//
            var foo = workerSignin.FirstOrDefault(r => r.dwccardnum == dwccardnum && EntityFunctions.DiffDays(r.dateforsignin, date) == 0 ? true : false);
            return foo;
        }

        public void CreateWorkerSignin(WorkerSignin signin, string user)
        {
            //Search for worker with matching card number
            Worker _wfound;
            _wfound = workerRepo.GetAllQ().FirstOrDefault(s => s.dwccardnum == signin.dwccardnum);
            if (_wfound != null)
            {
                signin.WorkerID = _wfound.ID;
            }
            //Search for duplicate signin for the same day
            int _sfound = 0;;
            _sfound = signinRepo.GetAllQ().Count(s => s.dateforsignin == signin.dateforsignin &&
                                                     s.dwccardnum == signin.dwccardnum);
            if (_sfound == 0)
            {
                signin.createdby(user);
                signinRepo.Add(signin);
                unitOfWork.Commit();
            }
        }
        //TODO: UnitTest DeleteWorkerSignin
        public void DeleteWorkerSignin(int id)
        {
            var workerSignin = signinRepo.GetById(id);
            signinRepo.Delete(workerSignin);
            unitOfWork.Commit();
        }
        //TODO: UnitTest SaveWorkerSignin
        public void SaveWorkerSignin()
        {
            unitOfWork.Commit();
        }
        public IEnumerable<WorkerSignin> GetSigninsForAssignment(DateTime date, string search, string order, int? displayStart, int? displayLength)
        {

            IQueryable<WorkerSignin> allWSI = signinRepo.GetAllQ();
            IQueryable<Worker> workers = workerRepo.GetAllQ();
            IQueryable<Person> persons = personRepo.GetAllQ();
                if (!string.IsNullOrEmpty(search))
                {
                    allWSI = allWSI.Where(s => s.dateforsignin == date)
                        .Join(workers, s => s.dwccardnum, w => w.dwccardnum, (s, w) => new { s, w })
                        //.Join(persons, oj => oj.w.ID, p => p.ID, (oj, p) => new { oj, p })
                        .Where(jj =>
                                    SqlFunctions.StringConvert((decimal)jj.s.dwccardnum).Contains(search) ||
                                    jj.w.Person.firstname1.Contains(search) ||
                                    jj.w.Person.firstname2.Contains(search) ||
                                    jj.w.Person.lastname1.Contains(search) ||
                                    jj.w.Person.lastname2.Contains(search))
                        .Select(a => a.s);
                }
            return allWSI.AsEnumerable();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public ServiceIndexView<WorkerSigninView> GetIndexView(DispatchOptions o)
        {
            //Get all the records
            IQueryable<WorkerSignin> queryableWSI = signinRepo.GetAllQ();
            //IQueryable<WorkerRequest> queryableWOWR;
            IEnumerable<WorkerSignin> enumWSI;
            IEnumerable<WorkerSigninView> enumWSIV;
            //Search based on search-bar string
            //DateTime parsedTime;
            if (o.date != null)
            {
                queryableWSI = queryableWSI.Where(p => EntityFunctions.DiffDays(p.dateforsignin, o.date) == 0 ? true : false);

            }
            // 
            // typeofwork ( DWC / HHH )
            //          
            if (o.typeofwork_grouping == lkup_dwc)
            {
                queryableWSI = queryableWSI
                                         .Where(wsi => wsi.worker.typeOfWorkID == lkup_dwc)
                                         .Select(wsi => wsi);
            }
            if (o.typeofwork_grouping == lkup_hhh)
            {
                queryableWSI = queryableWSI
                                         .Where(wsi => wsi.worker.typeOfWorkID == lkup_hhh)
                                         .Select(wsi => wsi);
            }    
            // 
            // wa_grouping
            //
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
                                                wsi => new { K1 = (int)wsi.WorkerID, K2 = (DateTime)EntityFunctions.TruncateTime(wsi.dateforsignin)}, 
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
                case "dateupdated": enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.dateupdated) : enumWSIV.OrderBy(p => p.dateupdated); break;
                default: enumWSIV = o.orderDescending ? enumWSIV.OrderByDescending(p => p.dateforsignin) : enumWSIV.OrderBy(p => p.dateforsignin); break;
            }
            //queryableWSI = queryableWSI.ToList();
            var filtered = enumWSIV.Count();
            //if (param.iDisplayLength > 0 && param.iDisplayStart > 0)
            if ((int)o.displayLength >= 0)
            enumWSIV = enumWSIV.Skip<WorkerSigninView>((int)o.displayStart).Take((int)o.displayLength);

            var total = signinRepo.GetAllQ().Count();

            return new ServiceIndexView<WorkerSigninView>
            {
                query = enumWSIV,
                filteredCount = filtered,
                totalCount = total
            };

        }

        //TODO: UnitTest getView
        public IEnumerable<WorkerSigninView> getView(DateTime date)
        {
            var signins = signinRepo.GetAllQ();
            var workers = workerRepo.GetAllQ();
            var persons = personRepo.GetAllQ();
            var s_to_w_query = from s in signins
                               where s.dateforsignin == date
                               join w in workers on s.dwccardnum equals w.dwccardnum into outer
                               from row in outer.DefaultIfEmpty()
                               join p in persons on row.ID equals p.ID into final
                               from finalrow in final.DefaultIfEmpty()
                               orderby s.datecreated descending
                               // Changed select statement to meet Linq/Entities requirement
                               // this form gets passed to SQL server
                               select new WorkerSigninView {
                                   dateforsignin = s == null ? DateTime.MinValue : s.dateforsignin,
                                   dwccardnum = s == null ? 0 : s.dwccardnum,
                                   signinID = s == null ? 0 : s.ID,
                                   firstname1 = finalrow == null ? null : finalrow.firstname1,
                                   firstname2 = finalrow == null ? null : finalrow.firstname2,
                                   lastname1 = finalrow == null ? null : finalrow.lastname1,
                                   lastname2 = finalrow == null ? null : finalrow.lastname2,
                                   imageID = finalrow == null ? null: finalrow.Worker.ImageID,
                                   expirationDate = finalrow.Worker.memberexpirationdate
                               };
            return s_to_w_query;

        }
        //TODO: UnitTest getImage
        public Image getImage(int cardrequest)
        { 
            Worker w_query = workerRepo.GetAllQ().Where(w => w.dwccardnum == cardrequest).AsEnumerable().FirstOrDefault();
            if (w_query == null) return null;
            if (w_query.ImageID != null)
            {
                return imageRepo.Get(i => i.ID == w_query.ImageID);
            }
            return null;
        }
        #endregion
        //TODO: UnitTest 
        public DateTime getExpireDate(int cardrequest)
        {
           //IEnumerable<Worker>  w_query = workerRepo.GetManyQ(w => w.dwccardnum == cardrequest).AsEnumerable();
            var workers = workerRepo.GetAllQ();
            Worker w_query = workers.Where(w => w.dwccardnum == cardrequest).AsEnumerable().FirstOrDefault();
            if (w_query == null)
            {
                //TODO: can't return null for datetime; better way to handle 'no record'?
                return DateTime.MinValue;
            }
            return w_query.memberexpirationdate;
        }
    }
    public static class String
    {
        public static bool ContainsOIC(this string source, string toCheck)
        {
            if (toCheck == null || source == null) return false;
            return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }

    }
}
