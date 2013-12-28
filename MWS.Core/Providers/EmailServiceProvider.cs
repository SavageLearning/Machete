using Machete.Data.Infrastructure;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWS.Core.Providers
{
    public interface IEmailServiceProvider
    {
        IEmailQueueManager GetQueueProvider();
        IDatabaseFactory GetFactory();
    }

    public class EmailServiceProvider : IEmailServiceProvider
    {
        private Func<IEmailQueueManager> _eqm;
        private Func<IDatabaseFactory> _dbf;

        public EmailServiceProvider(Func<IEmailQueueManager> f, Func<IDatabaseFactory> dbf)
        {
            _eqm = f;
            _dbf = dbf;
        }

        public IEmailQueueManager GetQueueProvider()
        {
            return _eqm();
        }

        public IDatabaseFactory GetFactory()
        {
            return _dbf();
        }
    }
}
