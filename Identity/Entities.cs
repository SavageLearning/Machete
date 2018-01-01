/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using Machete.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using IdSvr3 = IdentityServer3.Core;

namespace Machete.Identity
{
    public class UserStore : UserStore<MacheteUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public UserStore(MacheteContext ctx)
            : base(ctx)
        {
        }
    }

    public class UserManager : UserManager<MacheteUser, string>//, IUserEmailStore<MacheteUser, string>
    {
        public UserManager(UserStore store)
            : base(store)
        {
            this.ClaimsIdentityFactory = new ClaimsFactory();
        }
    }

    public class ClaimsFactory : ClaimsIdentityFactory<MacheteUser, string>
    {
        public ClaimsFactory()
        {
            this.UserIdClaimType = IdSvr3.Constants.ClaimTypes.Subject;
            this.UserNameClaimType = IdSvr3.Constants.ClaimTypes.PreferredUserName;
            this.RoleClaimType = IdSvr3.Constants.ClaimTypes.Role;
        }

        public override async Task<ClaimsIdentity> CreateAsync(
            UserManager<MacheteUser, string> manager, 
            MacheteUser user, 
            string authenticationType)
        {
            var ci = await base.CreateAsync(manager, user, authenticationType);
            if (!String.IsNullOrWhiteSpace(user.FirstName))
            {
                ci.AddClaim(new Claim("given_name", user.FirstName));
            }
            if (!String.IsNullOrWhiteSpace(user.LastName))
            {
                ci.AddClaim(new Claim("family_name", user.LastName));
            }
            if (!String.IsNullOrWhiteSpace(user.Email))
            {
                ci.AddClaim(new Claim("email", user.Email));
            }
            return ci;
        }
    }

    public class RoleStore : RoleStore<IdentityRole>
    {
        public RoleStore(MacheteContext ctx)
            : base(ctx)
        {
        }
    }

    public class RoleManager : RoleManager<IdentityRole>
    {
        public RoleManager(RoleStore store)
            : base(store)
        {
        }
    }


}