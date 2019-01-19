using System;
using Machete.Domain;
using Machete.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
    {
        private IWorkerSigninService _servWSI;
        private WorkerSignin _wsi;

        public FluentRecordBase AddWorkerSignin(
            Worker worker = null
        //DateTime? datecreated = null,
        //DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            _servWSI = container.GetRequiredService<IWorkerSigninService>();
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
