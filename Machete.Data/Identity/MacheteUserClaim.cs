using Microsoft.AspNetCore.Identity;

namespace Machete.Data.Identity
{
    public class MacheteUserClaim : IdentityUserClaim<string>
    {
        public virtual MacheteUser User { get; set; }
    }
}