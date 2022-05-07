using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Machete.Domain.EmbeddedAttributes
{
    /// <summary>
    /// A class to type-safety a text-as-json return value in the Configs
    /// Used to track an array of objects for the client to parse as JSON
    /// </summary>
    public class Confirm
    {
        public string name { get; set; }
        public string description { get; set; }
        public bool confirmed { get; set; } = false;
    }
}
