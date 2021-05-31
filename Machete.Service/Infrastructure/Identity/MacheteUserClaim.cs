using Microsoft.AspNetCore.Identity;

namespace Machete.Service.Identity
{
    public class MacheteUserClaim : IdentityUserClaim<string>
    {
        public virtual MacheteUser User { get; set; }
    }
}