using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class Survey : Record
    {
        public int ID { get; set; }
        public DateTime receivedDate { get; set; }
        public string generalComment { get; set; }
        // ICollection/lookup?
        public int qualityOfWork { get; set; }
        public int followsDirections { get; set; }
        public int attitude { get; set; }
        public int reliability { get; set; }
        public int transportProgram { get; set; }
    }
}
