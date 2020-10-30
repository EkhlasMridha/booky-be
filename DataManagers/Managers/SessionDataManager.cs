using DataManagers.Managers.interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataManagers.Managers
{
    class SessionDataManager : ISessionDataManager
    {
        private HotelDbContext _hotelDbContext;
        public SessionDataManager(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }
        public async Task<Session> CreateSessionAsync(Session session)
        {
            await _hotelDbContext.Session.AddAsync(session);
            await _hotelDbContext.SaveChangesAsync();

            return session;
        }

        public async Task DeleteSessionByAsync(Session session)
        {
            _hotelDbContext.Session.Remove(session);
            await _hotelDbContext.SaveChangesAsync();
        }

        public async Task<Session> GetSessionByIdAsync(string sessionId)
        {
            var session = await _hotelDbContext.Session.FirstOrDefaultAsync(s => s.Id == sessionId);

            return session;
        }

        public async Task<Session> GetSessionForUserandId(int userId, string sessionId)
        {
            var session = await _hotelDbContext.Session.FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);
            return session;
        }

        public async Task<Session> UpdateSessionAsync(Session session)
        {
            _hotelDbContext.Update(session);
            await _hotelDbContext.SaveChangesAsync();

            return session;
        }
    }
}
