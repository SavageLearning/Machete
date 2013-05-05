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
    }

    public class EmailService : ServiceBase<Email>, IEmailService
    {
        public EmailService(IEmailRepository emRepo, IUnitOfWork uow) : base(emRepo, uow)
        {
            this.logPrefix = "Email";
        }
    }
}
