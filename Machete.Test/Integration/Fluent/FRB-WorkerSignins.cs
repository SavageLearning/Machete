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
            // ACT (convert to UTC, as the client would)
            _wsi = _servWSI.CreateSignin(
                _w.dwccardnum,
                TimeZoneInfo
                    .ConvertTimeToUtc(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), ClientTimeZoneInfo),
                _user
            );
            return this;
        }

        public WorkerSignin ToWorkerSignin()
        {
            if (_wsi == null) AddWorkerSignin();
            return _wsi;
        }
    }
}
