using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Machete.Data;
using Machete.Service;
using Machete.Data.Infrastructure;

namespace Machete.Test.Controllers
{
    [TestClass]
    public class WorkerSigninControllerTests
    {
        Mock<IWorkerSigninService> _sserv ;
        Mock<IWorkerService> _wserv;
        Mock<IPersonService> _pserv;

        [TestMethod]
        public void WorkerSignin_getView_finds_joined_records()
        {
            //arrange
            _sserv = new Mock<IWorkerSigninService>();
            _wserv = new Mock<IWorkerService>();
            _pserv = new Mock<IPersonService>();
            //_service = new WorkerSigninService(_signinRepo.Object, _unitofwork);

        }
    }
}
