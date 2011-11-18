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
        [Authorize(Roles = "Manager, Administrator, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var allWorkers = workerService.GetWorkers(true);
            IEnumerable<Worker> filteredW;
            IEnumerable<Worker> orderedW;
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredW = workerService.GetWorkers(true)
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
                filteredW = allWorkers;
            }
            //
            //ORDER BY based on column selection
            //
            var orderDescending = true;
            if (param.sSortDir_0 == "asc") orderDescending = false;

            switch (param.sortColName())
            {                
                case "dwccardnum": orderedW = orderDescending ? filteredW.OrderByDescending(p => p.dwccardnum) : filteredW.OrderBy(p => p.dwccardnum); break;
                case "active": orderedW = orderDescending ? filteredW.OrderByDescending(p => p.active) : filteredW.OrderBy(p => p.active); break;
                case "firstname1": orderedW = orderDescending ? filteredW.OrderByDescending(p => p.Person.firstname1) : filteredW.OrderBy(p => p.Person.firstname1); break;
                case "firstname2": orderedW = orderDescending ? filteredW.OrderByDescending(p => p.Person.firstname2) : filteredW.OrderBy(p => p.Person.firstname2); break;
                case "lastname1": orderedW = orderDescending ? filteredW.OrderByDescending(p => p.Person.lastname1) : filteredW.OrderBy(p => p.Person.lastname1); break;
                case "lastname2": orderedW = orderDescending ? filteredW.OrderByDescending(p => p.Person.lastname2) : filteredW.OrderBy(p => p.Person.lastname2); break;
                case "memberexpirationdate": orderedW = orderDescending ? filteredW.OrderByDescending(p => p.memberexpirationdate) : filteredW.OrderBy(p => p.memberexpirationdate); break;
                default: orderedW = orderDescending ? filteredW.OrderByDescending(p => p.ID) : filteredW.OrderBy(p => p.ID); break;
            }
            //Sort the Persons based on column selection

            //Limit results to the display length and offset
            var displayPersons = orderedW.Skip(param.iDisplayStart)
                                              .Take(param.iDisplayLength);

            //return what's left to datatables
            var result = from p in displayPersons
                         select new{ 
                                     tabref = "/Worker/Edit/" + Convert.ToString(p.ID),
                                     tablabel =  p.Person.firstname1 + ' ' + p.Person.lastname1,
                                     WID =    p.ID.ToString(),
                                     dwccardnum =  Convert.ToString(p.dwccardnum),
                                     active =  Convert.ToString(p.active), 
                                     firstname1 = p.Person.firstname1, 
                                     firstname2 = p.Person.firstname2, 
                                     lastname1 = p.Person.lastname1, 
                                     lastname2 = p.Person.lastname2, 
                                     memberexpirationdate = Convert.ToString(p.memberexpirationdate)
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allWorkers.Count(),
                iTotalDisplayRecords = filteredW.Count(),
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

            return PartialView(_model);
        } 
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create(Worker _model, string userName)
        {
            if (!ModelState.IsValid)
            {
                levent.Level = LogLevel.Error; levent.Message = "ModelState invalid";
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
            return PartialView(_worker);
        }
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")]
        public ActionResult Edit(int id, Worker _model, string userName, HttpPostedFileBase imagefile)
        {
            Worker worker = workerService.GetWorker(id);
            // TODO: catch exceptions, notify user
            if (TryUpdateModel(worker))
            {
                if (imagefile != null)
                {
                    if (worker.ImageID != null)
                    {
                        Image image = imageServ.GetImage((int)worker.ImageID);
                        image.ImageMimeType = imagefile.ContentType;
                        image.parenttable = "Workers";
                        image.filename = imagefile.FileName;
                        image.recordkey = worker.ID.ToString();
                        image.ImageData = new byte[imagefile.ContentLength];
                        imagefile.InputStream.Read(image.ImageData,
                                                   0,
                                                   imagefile.ContentLength);
                        imageServ.SaveImage(image, this.User.Identity.Name);
                    }
                    else
                    {
                        Image image = new Image();
                        image.ImageMimeType = imagefile.ContentType;
                        image.parenttable = "Workers";
                        image.recordkey = worker.ID.ToString();
                        image.ImageData = new byte[imagefile.ContentLength];
                        imagefile.InputStream.Read(image.ImageData,
                                                   0,
                                                   imagefile.ContentLength);
                        Image newImage = imageServ.CreateImage(image, this.User.Identity.Name);
                        worker.ImageID = newImage.ID;
                    }
                }
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