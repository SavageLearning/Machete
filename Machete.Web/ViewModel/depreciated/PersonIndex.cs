using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Web.Models;
using Machete.Domain;

namespace Machete.Web.ViewModel
{
    public class PersonIndex
    {
        public Filter filter { get; set; }
        public Person person { get; set; }
        public IEnumerable<Person> persons { get; set; }
    }
}