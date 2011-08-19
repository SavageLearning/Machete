using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using NLog;
using System.Globalization;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

namespace Machete.Service
{
    public interface IWorkAssignmentService
    {
        IEnumerable<WorkAssignment> GetMany();
        IEnumerable<WorkAssignment> GetMany(Func<WorkAssignment, bool> where);
        IQueryable<WorkAssignment> GetManyQ(Func<WorkAssignment, bool> where);
        IQueryable<WorkAssignment> GetManyQ();
        //IEnumerable<WorkAssignment> GetManyByWO(int woID);
        IEnumerable<WorkAssignmentSummary> GetSummary();
        WorkAssignment Get(int id);
        WorkAssignment Create(WorkAssignment workAssignment, string user);
        void Delete(int id, string user);
        void Save(WorkAssignment workAssignment, string user);
        ServiceIndexView<WorkAssignment> GetIndexView(CultureInfo CI,
                                                        string search,
                                                        DateTime? date,
                                                        int? dwccardnum,
                                                        int? woid,
                                                        bool orderDescending,
                                                        int? displayStart,
                                                        int? displayLength,
                                                        string sortColName);
    }

    // Business logic for WorkAssignment record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class WorkAssignmentService : IWorkAssignmentService
    {
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkerRepository wRepo;
        private readonly IUnitOfWork unitOfWork;
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkAssignmentService", "");
        private WorkAssignment _workAssignment;
        //
        public WorkAssignmentService(IWorkAssignmentRepository waRepo, IWorkerRepository wRepo, IUnitOfWork unitOfWork)
        {
            this.waRepo = waRepo;
            this.unitOfWork = unitOfWork;
            this.wRepo = wRepo;
        }

        public IEnumerable<WorkAssignment> GetMany()
        {
            return waRepo.GetAll();
        }
        public IEnumerable<WorkAssignment> GetMany(Func<WorkAssignment, bool> where)
        {
            return waRepo.GetMany(where);
        }
        public IQueryable<WorkAssignment> GetManyQ()
        {
            return waRepo.GetAllQ().AsQueryable();
        }

        public IQueryable<WorkAssignment> GetManyQ(Func<WorkAssignment, bool> where)
        {

            return waRepo.GetAllQ().Where(where).AsQueryable();
        }

        public WorkAssignment Get(int id)
        {
            var workAssignment = waRepo.GetById(id);
            return workAssignment;
        }

        public ServiceIndexView<WorkAssignment> GetIndexView(
                                                    CultureInfo CI,
                                                    string search,
                                                    DateTime? date,
                                                    int? dwccardnum,
                                                    int? woid,
                                                    bool orderDescending,
                                                    int? displayStart,
                                                    int? displayLength,
                                                    string sortColName)
        {
            IQueryable<WorkAssignment> filteredWA = waRepo.GetAllQ();
            //Search based on search-bar string

            if (date != null)
            {
                filteredWA = filteredWA.Where(p => EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, date) == 0 ? true : false);
            }
            if (dwccardnum != null)
            {
                //Worker worker = wRepo.GetQ(w => w.dwccardnum == Convert.ToInt32(dwccardnum));

                //filteredWA = filteredWA
                //                .Where(wa => wa.englishLevelID <= worker.englishlevelID && (
                //                            wa.skillID.Equals(worker.skill1) ||
                //                            wa.skillID.Equals(worker.skill2) ||
                //                            wa.skillID.Equals(worker.skill3))
                //                            );


                //filteredWA = filteredWA.Join(Lookups,
                //    wa => wa.skillID,
                //    sk => sk.ID,
                //    (wa, sk) => new { wa, sk })
                //.Where(jj => jj.wa.englishLevelID <= worker.englishlevelID &&
                //            jj.sk.typeOfWorkID.Equals(worker.typeOfWorkID) && (
                //            jj.wa.skillID.Equals(worker.skill1) ||
                //            jj.wa.skillID.Equals(worker.skill2) ||
                //            jj.wa.skillID.Equals(worker.skill3) ||
                //            jj.sk.speciality == false)
                //            )
                //.Select(jj => jj.wa);
            }

            //if (woid != null) filteredWA = filteredWA.Where(p => p.workOrderID==woid);
            

            //if (!string.IsNullOrEmpty(search))
            //{
            //    filteredWA = filteredWA
            //        .Where(p => SqlFunctions.StringConvert((decimal)p.workOrder.paperOrderNum).Contains(search) ||

            //                    //p.workOrder.dateTimeofWork.ToString().ContainsOIC(param.sSearch) ||
            //                    p.description.Contains(search) ||
            //                    SqlFunctions.StringConvert((decimal)p.englishLevelID).Contains(search) ||
            //                    //Lookups.byID(p.skillID, CI.TwoLetterISOLanguageName).ContainsOIC(search) ||
            //                    //p.dateupdated.ToString().ContainsOIC(param.sSearch) ||
            //                    p.Updatedby.Contains(search));
            //}

            ////Sort the Persons based on column selection
            //var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            ////var sortColName = param.

            //var sortColName = param.sortColName();
            //Func<WorkAssignment, string> orderingFunction =
            //      (p => sortColName == "WOID" ? p.workOrder.ID.ToString() :
            //            sortColName == "WAID" ? p.ID.ToString() :
            //            sortColName == "pWAID" ? _getFullPseudoID(p) : 
            //            sortColName == "englishlevel" ? p.englishLevelID.ToString() :
            //            sortColName == "skill" ? Lookups.byID(p.skillID, culture) :
            //            sortColName == "hourlywage" ? p.hourlyWage.ToString() :
            //            sortColName == "hours" ? p.hours.ToString() :
            //            sortColName == "days" ? p.days.ToString() :
            //            sortColName == "description" ? p.description :
            //            sortColName == "dateTimeofWork" ? p.workOrder.dateTimeofWork.ToBinary().ToString() :
            //            sortColName == "earnings" ? Convert.ToString(p.hourlyWage * p.hours * p.days) :
            //            sortColName == "updatedby" ? p.Updatedby :
            //            p.dateupdated.ToBinary().ToString());

            //var sortDir = Request["sSortDir_0"];
            //if (sortDir == "asc")
            filteredWA = filteredWA.OrderBy(p => p.ID);
            //else
            //    sortedAssignments = filteredWA.OrderByDescending(orderingFunction);

            //Limit results to the display length and offset
           //if (displayLength != null && displayStart != null)
               filteredWA = filteredWA.Skip((int)displayStart)
                                                .Take((int)displayLength);
           //var filtered = filteredWA.Count();
           //var total = waRepo.GetAllQ().Count();
           return new ServiceIndexView<WorkAssignment>
           {
               query = filteredWA,
               filteredCount = 0,//filtered,
               totalCount = 0//total
           };

      }

        public IEnumerable<WorkAssignmentSummary> GetSummary()
        {
            var sum_query = from wa in waRepo.GetAll()
                            group wa by new
                            {
                                dateSoW = wa.workOrder.dateTimeofWork.ToString("MM/dd/yyyy"),
                                wa.workOrder.status
                            } into dayGroup
                            select new WorkAssignmentSummary()
                            {
                                date = dayGroup.Key.dateSoW,
                                status = dayGroup.Key.status,
                                count = dayGroup.Count()
                            };

            return sum_query;
        }


        public WorkAssignment Create(WorkAssignment workAssignment, string user)
        {
            workAssignment.createdby(user);
            _workAssignment = waRepo.Add(workAssignment);
            unitOfWork.Commit();
            _log(workAssignment.ID, user, "WorkAssignment created");
            return _workAssignment;
        }

        public void Delete(int id, string user)
        {
            var workAssignment = waRepo.GetById(id);
            waRepo.Delete(workAssignment);
            _log(id, user, "WorkAssignment deleted");
            unitOfWork.Commit();
        }

        public void Save(WorkAssignment workAssignment, string user)
        {
            workAssignment.updatedby(user);
            _log(workAssignment.ID, user, "WorkAssignment edited");
            unitOfWork.Commit();
        }

        private void _log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            levent.Properties["username"] = user;
            log.Log(levent);
        }
    }
}