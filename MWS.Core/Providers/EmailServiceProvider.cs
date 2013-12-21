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
        private Func<IEmailQueueManager> _func;
        private Func<IDatabaseFactory> _dbf;

        public EmailServiceProvider(Func<IEmailQueueManager> f, Func<IDatabaseFactory> dbf)
        {
            _func = f;
            _dbf = dbf;
        }

        public IEmailQueueManager GetQueueProvider()
        {
            return _func();
        }

        public IDatabaseFactory GetFactory()
        {
            return _dbf();
        }
    }
}
