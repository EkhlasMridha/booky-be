using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;

namespace DataManagers.Managers.interfaces
{
    public interface ISessionDataManager
    {
        public Task<Session> CreateSessionAsync(Session session);
        public Task<Session> GetSessionByIdAsync(string sessionId);
        public Task DeleteSessionByAsync(Session session);
        public Task<Session> UpdateSessionAsync(Session session);
        public Task<Session> GetSessionForUserandId(int userId, string sessionId);
    }
}
