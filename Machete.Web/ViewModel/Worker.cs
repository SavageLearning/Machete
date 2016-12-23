using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class Worker : Domain.Worker
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }
    }

    public class WorkerList
    {
        public int ID { get; set; }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string WID { get; set; }
        public string recordid { get; set; }
        public string dwccardnum { get; set; }
        public string active { get; set; }
        public string wkrStatus { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string memberexpirationdate { get; set; }
    }
}