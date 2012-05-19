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
        dTableList<Employer> GetIndexView(viewOptions o);
    }

    public class EmployerService : ServiceBase<Employer>, IEmployerService
    {
        private readonly IWorkOrderService _woServ;

        public EmployerService(IEmployerRepository repo, 
                               IWorkOrderService woServ,
                               IUnitOfWork unitOfWork)
                : base(repo, unitOfWork)
        {            
            this._woServ = woServ;
            this.logPrefix = "Employer";
        }

        public IEnumerable<WorkOrder> GetOrders(int id)
        {
            return _woServ.GetByEmployer(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public dTableList<Employer> GetIndexView(viewOptions o)
        {
            //Get all the records
            IQueryable<Employer> q = repo.GetAllQ();
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.search))
            {
                q = q.Where(p => //p.active.ToString().Contains(o.search) ||
                                p.name.Contains(o.search) ||
                                p.address1.Contains(o.search) ||
                                p.phone.Contains(o.search) ||
                                p.city.Contains(o.search));
            }
            //Sort the Persons based on column selection
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            //Limit results to the display length and offset
            q = q.Skip(o.displayStart).Take(o.displayLength);
            return new dTableList<Employer>
            {
                query = q,
                filteredCount = q.Count(),
                totalCount = repo.GetAllQ().Count()
            };
        }
    }
}