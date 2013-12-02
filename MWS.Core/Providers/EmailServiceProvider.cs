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
        public IEmailQueueManager GetQueueProvider();
    }

    public class EmailServiceProvider : IEmailServiceProvider
    {
        private Func<IEmailQueueManager> _func;

        public EmailServiceProvider(Func<IEmailQueueManager> f)
        { }

        public IEmailQueueManager GetQueueProvider()
        {
            return _func();
        }
    }
}
