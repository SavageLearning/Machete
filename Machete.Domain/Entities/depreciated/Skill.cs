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
        //TODO: Ask Hilary/Gabriel how complex to make this part....
        public int skillID { get; set; }
        public string skill { get; set; }
        public string level { get; set; }
        public string levelDesc { get; set; }
    }
}
