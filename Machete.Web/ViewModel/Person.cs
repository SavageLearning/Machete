using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class Person : Domain.Person
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }
    }

    public class PersonList
    {
        public int ID { get; set; }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string dwccardnum { get; set; }
        public string active { get; set; }
        public string status { get; set; }
        public string workerStatus { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string phone { get; set; }
        public string dateupdated { get; set; }
        public string updatedby { get; set; }
        public string recordid { get; set; }

    }
}