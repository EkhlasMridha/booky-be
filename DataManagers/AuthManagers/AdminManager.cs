using DataManagers.AuthManagers.interfaces;
using HelperServices;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataManagers.AuthManagers
{
    class AdminManager : IAdminManager
    {
        private HotelDbContext _hotelDbContext;
        public AdminManager(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }

        public async Task<Admin> ChangeBexioTokenAsync(int id,Admin admin)
        {
            var data = await _hotelDbContext.Admin.FirstOrDefaultAsync(a => a.UserId == id);

            data.BexioToken = admin.BexioToken;

            var update = await UpdateUser(data);

            return update;
        }

        public async Task<Admin> GetUserByIdAsync(int id)
        {
            var user = await _hotelDbContext.Admin.FirstOrDefaultAsync(a => a.UserId == id);

            return user;
        }

        public async Task<Admin> GetUserFromClaimAsync(ClaimsPrincipal claim)
        {
            string id = claim.FindFirst(ZeroClaims.UserId)?.Value;
            int userId = Int32.Parse(id);

            var user = await _hotelDbContext.Admin.FirstOrDefaultAsync(user => user.UserId == userId);
            return user;
        }

        public async Task ResetPassword(int userId,string newPassword)
        {
            var user =await  _hotelDbContext.Admin.FirstOrDefaultAsync(admin => admin.UserId == userId);
            if(user != null)
            {
                user.Password = newPassword;
                _hotelDbContext.Admin.Update(user);
                await _hotelDbContext.SaveChangesAsync();
            }
        }

        public async Task<Admin> SigninAsync(string username, string password)
        {
          var user = await _hotelDbContext.Admin.FirstOrDefaultAsync(user => user.UserName == username && user.Password == password);

            return user;
        }

        public async Task SignOutAsync(int userId)
        {
           var user = await _hotelDbContext.Admin.FirstOrDefaultAsync(admin => admin.UserId == userId);
            //user.RefreshToken = null;

            _hotelDbContext.Admin.Update(user);
            await _hotelDbContext.SaveChangesAsync();
        }

        public async Task<Admin> UpdateUser(Admin admin)
        {
            Utility.CheckNotNull(admin, nameof(admin));
            _hotelDbContext.Admin.Update(admin);
            await _hotelDbContext.SaveChangesAsync();

            return admin;
        }
    }
}
