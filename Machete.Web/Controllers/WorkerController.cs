using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using Machete.Domain;
using Machete.Helpers;
using Machete.Service;
using Machete.Web.ViewModel;
using Machete.Web.Models;
using Elmah;
using NLog;
using Machete.Web.Helpers;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkerController : Controller
    {
        private readonly IWorkerService workerService;
        private readonly IImageService imageServ;
        private bool foo1;
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkerController", "");

        public WorkerController(IWorkerService workerService, 
                                IPersonService personService,
                                IImageService  imageServ)
        {
            this.workerService = workerService;
            this.imageServ = imageServ;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];            
            ViewBag.races = Lookups.race(CI.TwoLetterISOLanguageName);
            ViewBag.languages = Lookups.language(CI.TwoLetterISOLanguageName);
            ViewBag.neighborhoods = Lookups.neighborhood(CI.TwoLetterISOLanguageName);
            ViewBag.incomes = Lookups.income(CI.TwoLetterISOLanguageName);
            ViewBag.maritalstatus = Lookups.maritalstatus(CI.TwoLetterISOLanguageName);
            ViewBag.Genders = Lookups.gender(CI.TwoLetterISOLanguageName);
            ViewBag.countriesoforigin = Lookups.countryoforigin(CI.TwoLetterISOLanguageName);
        }
        //
        // GET: /Worker/Index
        //
        #region index
        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk")]
        public ActionResult Index()
        {
            //WorkerIndex _view = new WorkerIndex();
            //_view.filter = new Models.Filter();
            //_view.filter.inactive = false;
            //_view.workers = workerService.GetWorkers(false);
            //return View(_view);
            return View();
        }

        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var allWorkers = workerService.GetWorkers(true);
            IEnumerable<Worker> filteredWorkers;
            IEnumerable<Worker> sortedWorkers;
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredWorkers = workerService.GetWorkers(true)
                    .Where(p => p.dwccardnum.ToString().ContainsOIC(param.sSearch) ||
                                p.active.ToString().ContainsOIC(param.sSearch) ||
                                p.Person.firstname1.ContainsOIC(param.sSearch) ||
                                p.Person.firstname2.ContainsOIC(param.sSearch) ||
                                p.Person.lastname1.ContainsOIC(param.sSearch) ||
                                p.Person.lastname2.ContainsOIC(param.sSearch) ||
                                p.memberexpirationdate.ToString().ContainsOIC(param.sSearch));
            }
            else
            {
                filteredWorkers = allWorkers;
            }
            //Sort the Persons based on column selection
            var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Worker, string> orderingFunction = (p => sortColIdx == 2 ? p.dwccardnum.ToString() :
                                                          sortColIdx == 3 ? p.active.ToString() :
                                                          sortColIdx == 4 ? p.Person.firstname1 :
                                                          sortColIdx == 5 ? p.Person.firstname2 :
                                                          sortColIdx == 6 ? p.Person.lastname1 :
                                                          sortColIdx == 7 ? p.Person.lastname2 :
                                                          p.memberexpirationdate.ToString());
            var sortDir = Request["sSortDir_0"];
            if (sortDir == "asc")
                sortedWorkers = filteredWorkers.OrderBy(orderingFunction);
            else
                sortedWorkers = filteredWorkers.OrderByDescending(orderingFunction);

            //Limit results to the display length and offset
            var displayPersons = sortedWorkers.Skip(param.iDisplayStart)
                                              .Take(param.iDisplayLength);

            //return what's left to datatables
            var result = from p in displayPersons
                         select new[] { "/Worker/Edit/" + Convert.ToString(p.ID),
                                        p.Person.firstname1 + ' ' + p.Person.lastname1,
                                        p.ID.ToString(),
                                        Convert.ToString(p.dwccardnum),
                                        Convert.ToString(p.active), 
                                        p.Person.firstname1, 
                                        p.Person.firstname2, 
                                        p.Person.lastname1, 
                                        p.Person.lastname2, 
                                        Convert.ToString(p.memberexpirationdate)
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allWorkers.Count(),
                iTotalDisplayRecords = filteredWorkers.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion
        //
        // GET: /Worker/Create
        //
        #region create
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create(int ID)
        {
            var _model = new Worker();
            _model.ID = ID;
            _model.RaceID = Lookups.raceDefault;
            _model.countryoforiginID = Lookups.countryoforiginDefault;
            _model.englishlevelID = Lookups.languageDefault;
            _model.neighborhoodID = Lookups.neighborhoodDefault;
            //_model.dateOfBirth = Date
            //Link person to work for EF to save

            //_model.worker.memberexpirationdate = null;
            return PartialView(_model);
        } 
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create(Worker _model, string userName)
        {
            if (!ModelState.IsValid)
            {
                levent.Level = LogLevel.Error; levent.Message = "ModelState invalid";
                //TODO will this log event work without record ID?
                //levent.Properties["RecordID"] = id; 
                log.Log(levent);
                return PartialView(_model);
            }

            workerService.CreateWorker(_model, userName);
            return PartialView(_model);
        }
        #endregion
        //
        // GET: /Worker/Edit/5
        //
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Edit(int id)
        {
            Worker _worker = workerService.GetWorker(id);
            //WorkerViewModel _model = new WorkerViewModel();
            //_model.worker = _worker;
            //_model.person = _worker.Person;
            //if (_worker.ImageID != null)
            //{
            //    _model.image = imageServ.GetImage((int)_worker.ImageID);
            //}
            //else
            //{
            //    _model.image = new Image();
            //}
            return PartialView(_worker);
        }
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")]
        public ActionResult Edit(int id, Worker _model, string userName)
        {
            Worker worker = workerService.GetWorker(id);
            //Person person = worker.Person;
            // TODO: catch exceptions, notify user
            foo1 = TryUpdateModel(worker);
            //foo2 = TryUpdateModel(person);
            if (foo1)
            {
                workerService.SaveWorker(worker, userName);
                return PartialView(worker);
            }
            else 
            {
                levent.Level = LogLevel.Error; levent.Message = "ModelState invalid";
                levent.Properties["RecordID"] = id; log.Log(levent);
                return View(_model);
            }
        }
        //
        //GET: /Worker/View/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, User")]
        public ActionResult View(int id)
        {
            Worker _worker = workerService.GetWorker(id);
            WorkerViewModel _model = new WorkerViewModel();
            _model.worker = _worker;
            _model.person = _worker.Person;
            if (_model.worker.ImageID != null)
            {
                _model.image = imageServ.GetImage((int)_worker.ImageID);
            }
            else
            {
                _model.image = new Image();
            }
            return View(_model);
        }
        //
        // GET: /Worker/Delete/5
        #region delete
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id)
        {
            var _worker = workerService.GetWorker(id);
            var _model = new WorkerViewModel();
            _model.worker = _worker;
            _model.person = _worker.Person;
            return View(_model);
        }
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id, string userName)
        {
            workerService.DeleteWorker(id, userName);
            levent.Level = LogLevel.Info; levent.Message = "Worker deleted";
            levent.Properties["RecordID"] = id; log.Log(levent);
            return RedirectToAction("Index");
        }
        #endregion
    }
}