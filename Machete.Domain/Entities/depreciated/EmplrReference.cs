using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class EmplrReference
    {
        [Key]
        public int EmplrRefID { get; set; }
        public string referencelabel { get; set; }
    }
}

