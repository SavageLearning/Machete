using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class WorkerSkill : Record
    {
        public int ID { get; set; }
        public int WorkerID { get; set; }
        public int SkillID { get; set; }
    }
}
