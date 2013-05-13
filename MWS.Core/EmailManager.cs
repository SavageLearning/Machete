using Machete.Data;
using Machete.Domain;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWS.Core
{
    public class EmailManager
    {
        IEmailService serv;

        public EmailManager(IEmailService eServ)
        {
            serv = eServ;
        }

        public void ProcessQueue()
        {
            var list = serv.GetAll().Where(e => e.status == Email.iReadyToSend);
            System.Diagnostics.Debug.WriteLine("Emails Ready to Send:"+list.Count().ToString());
        }
    }
}
