using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class Skill
    {
        public int ID { get; set; }
        public string skill { get; set; }
        public string level { get; set; }
    }
}
