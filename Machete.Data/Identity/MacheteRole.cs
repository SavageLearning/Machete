using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Machete.Data.Identity
{
    public class MacheteRole : IdentityRole
    {
        public virtual ICollection<MacheteUserRole> UserRoles { get; set; }
    }
}
