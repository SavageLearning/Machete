using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machete.Service
{
    public interface IEmailService : IService<Email>
    {
        dataTableResult<Email> GetIndexView(viewOptions o);
    }

    public class EmailService : ServiceBase<Email>, IEmailService
    {
        public EmailService(IEmailRepository emRepo, IUnitOfWork uow) : base(emRepo, uow)
        {
            this.logPrefix = "Email";
        }

        public dataTableResult<Email> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<Email>();
            IQueryable<Email> q = repo.GetAllQ();
            result.filteredCount = q.Count();
            result.totalCount = repo.GetAllQ().Count();
            result.query = q.OrderBy(e => e.ID).Skip(o.displayStart).Take(o.displayLength);
            return result;
        }
    }
}
