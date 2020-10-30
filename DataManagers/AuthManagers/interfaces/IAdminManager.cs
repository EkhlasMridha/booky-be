using Models.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataManagers.AuthManagers.interfaces
{
    public interface IAdminManager
    {
        public Task<Admin> SigninAsync(string username, string password);
        public Task SignOutAsync(int userId);
        public Task ResetPassword(int userId, string newPassword);
        public Task<Admin> UpdateUser(Admin admin);

        public Task<Admin> GetUserFromClaimAsync(ClaimsPrincipal claim);
        public Task<Admin> ChangeBexioTokenAsync(int id,Admin admin);

        public Task<Admin> GetUserByIdAsync(int id);
    }
}
