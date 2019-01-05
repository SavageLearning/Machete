using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Machete.Service
{
    public interface IEmailService : IService<Email>
    {
        Email GetLatestConfirmEmailBy(int woid);
        Email GetExclusive(int eid, string user);
        Email Create(Email email, string userName, int? woid = null);
        Email Duplicate(int id, int? woid, string userName);
        WorkOrder GetAssociatedWorkOrderFor(Email email);
        WorkOrder GetAssociatedWorkOrderFor(int woid);
        new IEnumerable<Email> GetMany(Func<Email, bool> predicate);
        IEnumerable<Email> GetEmailsToSend();
        dataTableResult<Email> GetIndexView(viewOptions o);
    }

    public class EmailService : ServiceBase<Email>, IEmailService
    {
        IWorkOrderService _woServ;
        IEmailConfig _emCfg;
        public EmailService(IEmailRepository emRepo, IWorkOrderService woServ, IUnitOfWork uow, IEmailConfig emCfg) : base(emRepo, uow)
        {
            this.logPrefix = "Email";
            _woServ = woServ;
            _emCfg = emCfg;
        }

        public override Email Create(Email email, string userName)
        {
            return Create(email, userName, null);
        }

        public Email Create(Email email, string userName, int? woid = null)
        {
            if (email.statusID == Email.iReadyToSend)
            {
                SendSmtpSimple(email);
            }
            Email newEmail;
            newEmail = base.Create(email, userName);
            //newEmail = Get(newEmail.ID);
            if (woid != null)
            {
                WorkOrder wo = _woServ.Get((int)woid);
                newEmail.WorkOrders.Add(wo);
            }
            uow.Commit();
            return newEmail;
        }

        public Email Duplicate(int id, int? woid, string userName)
        {
            Email e = Get(id);
            Email duplicate = e;
            duplicate.statusID = Email.iPending;
            duplicate.lastAttempt = null;
            duplicate.transmitAttempts = 0;

            return Create(duplicate, userName, woid);

        }

        public override void Save(Email email, string userName)
        {
            if (email.statusID == Email.iSent) { return; } 
            if (email.statusID == Email.iReadyToSend)
            {
                SendSmtpSimple(email);
            }
            base.Save(email, userName);
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
        /// <summary>
        /// Makes sure an email being edited doesn't get sent by service during edit
        /// </summary>
        /// <param name="eid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
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
            e.updatedByUser(user);
            log(e.ID, user, logPrefix + " get exclusive for email");
            uow.Commit();
            return e;
        }

        new public IEnumerable<Email> GetMany(Func<Email, bool> predicate)
        {
            return repo.GetManyQ(predicate);
        }

        public IEnumerable<Email> GetEmailsToSend()
        {
            return ((EmailRepository)repo).GetEmailsToSend();
        }

        public dataTableResult<Email> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<Email>();
            IQueryable<Email> q = repo.GetAllQ();
            if (o.woid > 0) IndexViewBase.filterOnWorkorder(o, ref q);
            if (o.emailID.HasValue) IndexViewBase.filterOnID(o, ref q);
            if (o.EmployerID.HasValue) IndexViewBase.filterOnEmployer(o, ref q);
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);

            IEnumerable<Email> e = q.AsEnumerable();
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref e);
            result.filteredCount = e.Count();
            result.totalCount = repo.GetAllQ().Count();
            result.query = e.Skip(o.displayStart).Take(o.displayLength);
            return result;
        }

        public void SendSmtpSimple(Email email)
        {
            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential =
                //new NetworkCredential("machete-dcd9269ea5afcbe7", "e16356c22f5a4c13");
                new NetworkCredential(_emCfg.userName, _emCfg.password);
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(_emCfg.fromAddress);

            smtpClient.Host = _emCfg.host;
            smtpClient.Port = _emCfg.port;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.EnableSsl = _emCfg.enableSSL;

            message.From = fromAddress;
            message.Subject = email.subject;
            message.IsBodyHtml = true;
            message.Body = email.body;
            message.To.Add(email.emailTo);
            if (email.attachment != null)
            {
                var a = Attachment.CreateAttachmentFromString(email.attachment, email.attachmentContentType);
                a.Name = "Order.html";
                message.Attachments.Add(a);
            }

            smtpClient.Send(message);
            email.statusID = Email.iSent;
            email.emailFrom = fromAddress.Address;
        }
    }
}
