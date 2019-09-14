using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Machete.Data.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Machete.Data.Identity
{
    public class MacheteRoleStore : IQueryableRoleStore<MacheteRole>
    {
        private MacheteContext _dataContext;
        private DbSet<MacheteRole> _roles;

        public MacheteRoleStore(IDatabaseFactory factory)
        {
            _dataContext = factory.Get();
            _roles = _dataContext.Roles;
        }

        public MacheteRoleStore(MacheteContext assignedContext, bool isSeedInstance)
        {
            _dataContext = assignedContext;
            _roles = _dataContext.Roles;
        }

        public Task<IdentityResult> CreateAsync(MacheteRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role)); // new up your own; follows pattern?

            var unused = _roles.Add(role);

            _dataContext.SaveChanges();

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(MacheteRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = _roles.Update(role);

            _dataContext.SaveChanges();

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(MacheteRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = _roles.Remove(role);

            _dataContext.SaveChanges();
            
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<string> GetRoleIdAsync(MacheteRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(MacheteRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(MacheteRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            role.Name = roleName;
            //role.NormalizedName = roleName.ToUpper();

            _roles.Update(role);

            //_dataContext.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(MacheteRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(MacheteRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            
            role.NormalizedName = roleName.ToUpper();

            _roles.Update(role);

            //_dataContext.SaveChanges();
            
            return Task.CompletedTask;
        }

        public Task<MacheteRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (String.IsNullOrEmpty(roleId)) throw new ArgumentNullException(nameof(roleId));

            var identityRole = _roles.FirstOrDefault(role => role.Id == roleId);
            
            if (identityRole == null) throw new NullReferenceException("Could not populate IdentityRole");
            
            return Task.FromResult(identityRole);
        }

        public Task<MacheteRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (String.IsNullOrEmpty(normalizedRoleName)) throw new ArgumentNullException(nameof(normalizedRoleName));

            var identityRole = _roles.FirstOrDefault(role => role.NormalizedName == normalizedRoleName);
            
            //if (identityRole == null) throw new NullReferenceException("Could not populate IdentityRole");
            
            return Task.FromResult(identityRole);
        }
        
        public IQueryable<MacheteRole> Roles => _roles;

        public void Dispose() { }
    }
}
