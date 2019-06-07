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

        public FluentRecordBase AddWorkerSignin(Worker worker = null)
        {
            // ARRANGE
            _servWSI = container.GetRequiredService<IWorkerSigninService>();
            var _w = worker ?? AddWorker();

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
