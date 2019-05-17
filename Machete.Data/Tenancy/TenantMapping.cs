using System;
using System.Collections.Generic;

namespace Machete.Data.Tenancy
{
    public class TenantMapping
    {
        public bool AllowDefault { get; set; }
        public string Default { get; set; }
        public Dictionary<string, string> Tenants { get; set; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
    }
}
