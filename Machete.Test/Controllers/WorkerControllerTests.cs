using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web;
using Machete.Web.Controllers;
using Moq;
using NUnit.Framework;

//namespace Machete.Test
//{
//    [TestFixture]
//    public class WorkerControllerTest
//    {
//        Mock<IWorkerRepository> _workerRepository;
//        Mock<IRaceRepository> _raceRepository;
//        IWorkerService _workerService;
//        IRaceService _raceService;
//        IUnitOfWork _unitofwork;


//        [SetUp]
//        public void Setup()
//        {
//            _workerRepository = new Mock<IWorkerRepository>();
//            _workerService = new WorkerService(_workerRepository.Object, _unitofwork);
//            //_personService = mew PersonService(_
//            _raceRepository = new Mock<IRaceRepository>();
//            _raceService = new RaceService(_raceRepository.Object);
//        }

//        [Test]
//        public void Create_Category()
//        {
//            IQueryable<Worker> fakeWorkers = new List<Worker> {
//                new Worker {    //ID = 7,
//                                carinsurance = true,
//                                countryoforigin = "USA",
//                                disabilitydesc = "foo",
//                                disabled = true,
//                                driverslicense = true,
//                                dwccardnum = 1,
//                                emcontoriginname = "Barak Obama",
//                                emcontoriginphone = "1234567890",
//                                emcontoriginrelation = "friend",
//                                emcontUSAname = "Bill Clinton",
//                                emcontUSAphone = "1234567890",
//                                emcontUSArelation = "idol",
//                                englishlevelID = 1,
//                                height = "too tall",
//                                immigrantrefugee = true,
//                                incomeID = 1,
//                                livealone = true,
//                                livewithchildren = true,
//                                maritalstatus = "S",
//                                memberexpirationdate = DateTime.Now,
//                                numofchildren = 0,
//                                RaceID = 1,
//                                raceother = "fabulous",
//                                recentarrival = true,
//                                neighborhoodID = 1,
//                                weight = "too big",
//                                datecreated = DateTime.Now,
//                                dateupdated = DateTime.Now,
//                                dateinUSA = DateTime.Now,
//                                dateinseattle = DateTime.Now,
//                                insuranceexpiration = DateTime.Now,
//                                licenseexpirationdate = DateTime.Now
//                }
//            }.AsQueryable();


//            _workerRepository.Setup(x => x.GetAll()).Returns(fakeWorkers);

//            //WorkerController controller = new WorkerController(_workerService, _raceService);
//            // Act
//            //ViewResult result = controller.Create(null) as ViewResult;
//            // Assert
//            //Assert.IsNotNull(result, "View Result is null");
//            //Assert.IsInstanceOf(typeof(PagedList<Category>), result.ViewData.Model, "Wrong ViewModel");
//            //var categories = result.ViewData.Model as PagedList<Category>;
//            //Assert.AreEqual(3, categories.Count, "Got wrong number of Categories");
//            //Assert.AreEqual(0, (int)categories.PageIndex, "Wrong page Index");
//            //Assert.AreEqual(1, (int)categories.PageNumber, "Wrong  page Number");

//        }
//    }
//}
////[TestMethod]
////public void WorkerController_Create()
////{
////    DatabaseFactory _dbfactory = new DatabaseFactory();
////    WorkerRepository _workerrepo = new WorkerRepository(_dbfactory);
////    RaceRepository _racerepo = new RaceRepository(_dbfactory);
////    PersonRepository _personrepo = new PersonRepository(_dbfactory);
////    UnitOfWork _iunitofwork = new UnitOfWork(_dbfactory);
////    WorkerService _service = new WorkerService(_workerrepo, _iunitofwork);
////    PersonService _pService = new PersonService(_personrepo, _iunitofwork);
////    RaceService _race = new RaceService(_racerepo);
////    var Personcontroller = new PersonController(_pService);
////    var Workcontroller = new WorkerController(_service, _race);
////    //var pResult = Personcontroller.Create(_person) as ViewResult;
////    //var result = Workcontroller.Create(_worker) as ViewResult;

////}