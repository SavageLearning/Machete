using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class TypeOfWork
    {
        [Key]
        public int workID { get; set; }
        public string worklabel { get; set; }
    }
}
