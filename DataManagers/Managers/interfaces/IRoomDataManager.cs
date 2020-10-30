using Models.Dtos;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataManagers.Managers.interfaces
{
    public interface IRoomDataManager
    {
        public Task<List<Room>> GetAllRoom();
        public Task<List<Room>> GetDataByMonth(DateTime dateTime);
        public Task<Room> GetRoomById(int id);
        public Task<Room> CreateRoom(Room room);
        public Task<Room> UpdateRoomAsync(Room room);
        public Task DeleteRoomAsync(Room room);
    }

}
