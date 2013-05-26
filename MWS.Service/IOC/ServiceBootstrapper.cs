using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Service;
using Microsoft.Practices.Unity;
using MWS.Core;
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MWS.Service
{
    public class ServiceBootstrapper
    {
        public IUnityContainer Build()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IEmailManager, EmailManager>();
            container.RegisterInstance<MacheteWindowsService>(new MacheteWindowsService(container));
            container.RegisterType<IEmailRepository, EmailRepository>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IDatabaseFactory, DatabaseFactory>();
            return container;
        }
    }
}
