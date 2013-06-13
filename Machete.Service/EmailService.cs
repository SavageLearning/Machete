using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Machete.Service
{
    public interface IEmailService : IService<Email>
    {
        Email GetLatestConfirmEmailBy(int woid);
        Email GetExclusive(int eid, string user);
        Email CreateWithWorkorder(Email email, int woid, string userName);
        WorkOrder GetAssociatedWorkOrderFor(Email email);
        WorkOrder GetAssociatedWorkOrderFor(int woid);
        IEnumerable<Email> GetMany(Func<Email, bool> predicate);
        IEnumerable<Email> GetEmailsToSend();
        dataTableResult<Email> GetIndexView(viewOptions o);
    }

    public class EmailService : ServiceBase<Email>, IEmailService
    {
        IWorkOrderService _woServ;

        public EmailService(IEmailRepository emRepo, IWorkOrderService woServ, IUnitOfWork uow) : base(emRepo, uow)
        {
            this.logPrefix = "Email";
            _woServ = woServ;
        }

        public Email CreateWithWorkorder(Email email, int woid, string userName)
        {
            WorkOrder wo = _woServ.Get(woid);
            Email newEmail;
            newEmail = Create(email, userName);
            newEmail = Get(newEmail.ID);
            //var newJoiner = new JoinWorkorderEmail();
            //newJoiner.WorkOrderID = woid;
            //newJoiner.EmailID = newEmail.ID;
            //newJoiner.Createdby = userName;
            //newJoiner.Updatedby = userName;
            //newJoiner.datecreated = DateTime.Now;
            //newJoiner.dateupdated = DateTime.Now;
            newEmail.WorkOrders = new Collection<WorkOrder>();
            newEmail.WorkOrders.Add(wo);
            //newEmail.JoinWorkorderEmails = new  Collection<JoinWorkorderEmail>();
            //newEmail.JoinWorkorderEmails.Add(newJoiner);
            uow.Commit();
            return newEmail;
        }

        public Email GetLatestConfirmEmailBy(int woid)
        {
            var wo = _woServ.Get(woid);
            if (wo == null) throw new MacheteServiceException("Cannot find workorder.");
            var email = wo.Emails.OrderByDescending(e => e.datecreated).FirstOrDefault();
            if (email == null) {return null;}
            return email;
        }

        public WorkOrder GetAssociatedWorkOrderFor(Email email)
        {
            if (!email.isAssociatedToWorkOrder) throw new MacheteServiceException("No WorkOrder associated with Email");
            try
            {
                return email.WorkOrders.Single();
            }
            catch (Exception ex)
            {
                throw new MacheteIntegrityException("Email is associated with more than one Workorder", ex);
            }
        }

        public WorkOrder GetAssociatedWorkOrderFor(int woid)
        {
            return _woServ.Get(woid);
        }

        public Email GetExclusive(int eid, string user)
        {
            var e =  repo.GetById(eid);
            if (e.statusID == Email.iSending ||
                e.statusID == Email.iSent)
            {
                return null;
            }
            // user will have to re-send when editing a ReadyToSend
            if (e.statusID == Email.iReadyToSend)
            {
                e.statusID = Email.iPending;
            }
            // transmit errors will remain as error re-sent
            e.updatedby(user);
            log(e.ID, user, logPrefix + " get exclusive for email");
            uow.Commit();
            return e;
        }

        public IEnumerable<Email> GetMany(Func<Email, bool> predicate)
        {
            return repo.GetMany(predicate);
        }

        public IEnumerable<Email> GetEmailsToSend()
        {
            return repo.GetManyQ()
                       .Where(e => e.statusID == Email.iReadyToSend ||
                           (e.statusID == Email.iTransmitError && e.transmitAttempts < 10)
                           )
                       .ToList();
        }

        public dataTableResult<Email> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<Email>();
            IQueryable<Email> q = repo.GetAllQ();
            if (o.woid > 0) IndexViewBase.filterOnWorkorder(o, ref q);
            if (o.emailID.HasValue) IndexViewBase.filterOnID(o, ref q);
            if (o.EmployerID.HasValue) IndexViewBase.filterOnEmployer(o, ref q);

            IEnumerable<Email> e = q.AsEnumerable();
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref e);
            result.filteredCount = e.Count();
            result.totalCount = repo.GetAllQ().Count();
            result.query = e.Skip(o.displayStart).Take(o.displayLength);
            return result;
        }
    }
}
