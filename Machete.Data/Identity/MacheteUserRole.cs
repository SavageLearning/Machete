using Microsoft.AspNetCore.Identity;

namespace Machete.Data.Identity
{
    public class MacheteUserRole : IdentityUserRole<string>
    {
        public virtual MacheteUser User { get; set; }
        public virtual MacheteRole Role { get; set;  }
    }
}