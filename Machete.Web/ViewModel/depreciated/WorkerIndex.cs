using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Web.Models;
using Machete.Domain;

namespace Machete.Web.ViewModel
{
    public class WorkerIndex
    {
        public Filter filter { get; set; }
        public IEnumerable<Worker> workers { get; set; }
    }
}