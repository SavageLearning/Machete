using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using NLog;

namespace Machete.Service
{
    public interface IEmployerService : IService<Employer>
    {
        IEnumerable<WorkOrder> GetOrders(int id);
    }

    public class EmployerService : ServiceBase<Employer>, IEmployerService
    {
        private readonly IWorkOrderService _woServ;

        public EmployerService(IEmployerRepository employerRepository, 
                               IWorkOrderService workorderService,
                               IUnitOfWork unitOfWork)
                : base(employerRepository, unitOfWork)
        {            
            this._woServ = workorderService;
            this.logPrefix = "Employer";
        }

        public IEnumerable<WorkOrder> GetOrders(int id)
        {
            return _woServ.GetWorkOrders(id);
        }
    }
}