using Machete.Data;
using Machete.Domain;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Test.Integration
{
    public partial class FluentRecordBase : IDisposable
    {
        private WorkerSigninRepository _repoWSI;
        private WorkerSigninService _servWSI;
        private WorkerSignin _wsi;


        public FluentRecordBase AddRepoWorkerSignin()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoWSI = new WorkerSigninRepository(_dbFactory);
            return this;
        }

        public WorkerSigninRepository ToRepoWorkerSignin()
        {
            if (_repoWSI == null) AddRepoWorkerSignin();
            return _repoWSI;
        }

        public FluentRecordBase AddServWorkerSignin()
        {
            //
            // DEPENDENCIES
            if (_repoWSI == null) AddRepoWorkerSignin();
            if (_servW == null) AddServWorker();
            if (_servL == null) AddServImage();
            if (_servAS == null) AddServWorkerRequest();
            if (_uow == null) AddUOW();
            if (_webMap == null) AddMapper();
            if (_servC == null) AddServConfig();
            _servWSI = new WorkerSigninService(_repoWSI, _servW, _servI, _servWR, _uow, _webMap, _servC);
            return this;
        }

        public WorkerSigninService ToServWorkerSignin()
        {
            if (_servWSI == null) AddServWorkerSignin();
            return _servWSI;
        }

        public FluentRecordBase AddWorkerSignin(
            Worker worker = null
        //DateTime? datecreated = null,
        //DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_servWSI == null) AddServWorkerSignin();
            if (worker != null) _w = worker;
            if (_w == null) AddWorker();
            //
            // ARRANGE

            //
            // ACT
            _wsi = _servWSI.CreateSignin(_w.dwccardnum, DateTime.Now, _user);
            return this;
        }

        public WorkerSignin ToWorkerSignin()
        {
            if (_wsi == null) AddWorkerSignin();
            return _wsi;
        }


    }
}
