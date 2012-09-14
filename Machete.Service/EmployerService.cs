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
        dataTableResult<Employer> GetIndexView(viewOptions o);
    }

    public class EmployerService : ServiceBase<Employer>, IEmployerService
    {
        private readonly IWorkOrderService woServ;

        public EmployerService(IEmployerRepository repo, 
                               IWorkOrderService woServ,
                               IUnitOfWork unitOfWork)
                : base(repo, unitOfWork)
        {
            this.woServ = woServ;
            this.logPrefix = "Employer";
        }
        public IEnumerable<WorkOrder> GetOrders(int id)
        {
            return woServ.GetByEmployer(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public dataTableResult<Employer> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<Employer>();
            //Get all the records
            IQueryable<Employer> q = repo.GetAllQ();
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);
            //Sort the Persons based on column selection
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            //Limit results to the display length and offset
            result.filteredCount = q.Count();
            result.totalCount = repo.GetAllQ().Count();
            result.query = q.Skip(o.displayStart).Take(o.displayLength);
            return result;            
        }
    }
}