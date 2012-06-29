using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using NLog;
using System.Globalization;

namespace Machete.Service
{
    public interface ILookupService : IService<Lookup>
    {
        IEnumerable<Lookup> GetIndexView(viewOptions o);
    }

    // Business logic for Lookup record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class LookupService : ServiceBase<Lookup>, ILookupService
    {
        private readonly ILookupRepository lrepo;
        public LookupService(ILookupRepository lRepo,
                             IUnitOfWork unitOfWork)
            : base(lRepo, unitOfWork)
        {
            this.lrepo = lRepo;
            this.logPrefix = "Lookup";
        }

        public IEnumerable<Lookup> GetIndexView(viewOptions o)
        {
            //Get all the records
            IQueryable<Lookup> q = repo.GetAllQ();
            //
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.search)) IndexViewBase.search(o, ref q);
            if (!string.IsNullOrEmpty(o.category)) IndexViewBase.byCategory(o, ref q);
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);

            q = q.Skip<Lookup>(o.displayStart).Take(o.displayLength);
            return q;
        }
        public override Lookup Create(Lookup record, string user)
        {
            // Only one record can be true in a given category
            if (record.selected == true)
            {
                lrepo.clearSelected(record.category);
                record.selected = true;
            }

            return base.Create(record, user);
        }
        public override void Save(Lookup record, string user)
        {
            // Only one record can be true in a given category
            if (record.selected == true)
            {
                lrepo.clearSelected(record.category);
                record.selected = true;
            }
            base.Save(record, user);
        }
    }
}