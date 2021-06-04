using Machete.Service;
using Machete.Service.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

    public class EmailService : ServiceBase2<Email>, IEmailService
    {
        IEmailConfig _emCfg;
        public EmailService(IDatabaseFactory db, IMapper map, IEmailConfig emCfg) : base(db, map)
        {
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

            var newEmail = base.Create(email, userName);

            if (woid != null)
            {
                WorkOrder wo = db.WorkOrders.Find((int)woid);
                newEmail.WorkOrders.Add(wo);
            }
            db.SaveChanges();
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
            var wo = db.WorkOrders.Find(woid);
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
            return db.WorkOrders.Find(woid);
        }
        /// <summary>
        /// Makes sure an email being edited doesn't get sent by service during edit
        /// </summary>
        /// <param name="eid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Email GetExclusive(int eid, string user)
        {
            var e =  dbset.Find(eid);
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
            db.SaveChanges();
            return e;
        }

        new public IEnumerable<Email> GetMany(Func<Email, bool> predicate)
        {
            return dbset.Where(predicate);
        }

        public IEnumerable<Email> GetEmailsToSend()
        {
            //var emails = dbset.Where(e => e.statusID == Email.iReadyToSend ||
            //               (e.statusID == Email.iTransmitError && e.transmitAttempts < 10)
            //               );
            var sb = new StringBuilder();
            sb.AppendFormat("select * from Emails e  with (UPDLOCK) where e.statusID = {0} or ", Email.iReadyToSend);
            sb.AppendFormat("(e.statusID = {0} and e.transmitAttempts < {1})", Email.iTransmitError, Email.iTransmitAttempts);
            var set = (DbSet<Email>)dbset;
            return set.FromSqlRaw(sb.ToString()).AsEnumerable();
        }

        public dataTableResult<Email> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<Email>();
            IQueryable<Email> q = dbset.AsNoTracking().AsQueryable();
            if (o.woid > 0) IndexViewBase.filterOnWorkorder(o, ref q);
            if (o.emailID.HasValue) IndexViewBase.filterOnID(o, ref q);
            if (o.EmployerID.HasValue) IndexViewBase.filterOnEmployer(o, ref q);
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);

            IEnumerable<Email> e = q.AsEnumerable();
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref e);
            result.filteredCount = e.Count();
            result.totalCount = dbset.AsNoTracking().AsQueryable().Count();
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
