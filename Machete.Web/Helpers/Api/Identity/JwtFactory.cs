using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Machete.Data;
using Machete.Data.Identity;
using Machete.Web.ViewModel.Api.Identity;

namespace Machete.Web.Helpers.Api.Identity
{
    // https://fullstackmark.com/post/13/jwt-authentication-with-aspnet-core-2-web-api-angular-5-net-core-identity-and-facebook-login
    // https://github.com/mmacneil/AngularASPNETCore2WebApiAuth/tree/master/src/Auth (MIT)
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(ClaimsIdentity claimsIdentity, JwtIssuerOptions jwtOptions);
        Task<ClaimsIdentity> GenerateClaimsIdentity(MacheteUser subject, JwtIssuerOptions options);
    }
    public class JwtFactory : IJwtFactory
    {
        public async Task<string> GenerateEncodedToken(ClaimsIdentity claimsIdentity, JwtIssuerOptions jwtOptions)
        {
            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(claims: claimsIdentity.Claims, signingCredentials: jwtOptions.SigningCredentials)
            ));
        }

        public async Task<ClaimsIdentity> GenerateClaimsIdentity(MacheteUser subject, JwtIssuerOptions jwtOptions)
        {
            var preferredUserName = $"{subject.FirstName} {subject.LastName}";
            if (string.IsNullOrWhiteSpace(preferredUserName)) preferredUserName = subject.UserName;

            var claims = new List<Claim>
            {
                new Claim("id", subject.Id),
                new Claim(JwtRegisteredClaimNames.Iss, jwtOptions.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, jwtOptions.Audience),
                new Claim(JwtRegisteredClaimNames.Exp, UnixEpochDateFor(jwtOptions.Expiration)),
                new Claim(JwtRegisteredClaimNames.Nbf, UnixEpochDateFor(jwtOptions.NotBefore)),
                new Claim(JwtRegisteredClaimNames.Nonce, jwtOptions.Nonce), 
                new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, UnixEpochDateFor(jwtOptions.IssuedAt)),
                //new Claim(JwtRegisteredClaimNames.AtHash, ???),
                //new Claim(JwtRegisteredClaimNames.Sid, ???),
                new Claim(JwtRegisteredClaimNames.Sub, subject.Id),
                new Claim(JwtRegisteredClaimNames.AuthTime, UnixEpochDateFor(jwtOptions.IssuedAt)),
                new Claim(JwtRegisteredClaimNames.Amr, "cookies") // TODO determine programatically
            };
            claims.AddRange(subject.UserRoles.Select(role => new Claim("role", role.Role.Name))); //TODO test
            claims.Add(new Claim("preferredUserName", preferredUserName));

            return new ClaimsIdentity(new GenericIdentity(subject.UserName, "Token"), claims);
        }
        
        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static string UnixEpochDateFor(DateTime date)
            => ((long) Math.Round((date.ToUniversalTime() -
                                  new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds)).ToString(CultureInfo.InvariantCulture);
    }
}
