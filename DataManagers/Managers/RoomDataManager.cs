using DataManagers.Managers.interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace DataManagers.Managers
{
    class RoomDataManager : IRoomDataManager
    {
        private HotelDbContext _hotelDbContext;
        public RoomDataManager(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }

        public async Task<Room> CreateRoom(Room room)
        {
            _hotelDbContext.Room.Add(room);
            await _hotelDbContext.SaveChangesAsync();

            return room;
        }

        public async Task DeleteRoomAsync(Room room)
        {
            var roomFinal = await _hotelDbContext.Room.Include(b => b.BookedRooms).FirstOrDefaultAsync(a=>a.Id == room.Id);
            _hotelDbContext.Room.Remove(roomFinal);
            await _hotelDbContext.SaveChangesAsync();
        }

        public async Task<List<Room>> GetAllRoom()
        {
            var room = await _hotelDbContext.Room.Include(c => c.BookedRooms).ThenInclude(cs => cs.Booking).ThenInclude(st=>st.state).ToListAsync();

            return room;
        }

        public async Task<List<Room>> GetDataByMonth(DateTime dateTime)
        {
            var month = dateTime.Month;

            var bookingRoom = await _hotelDbContext.Room.IncludeFilter(a => a.BookedRooms.Where(b => b.Booking.Book_From.Month == month || b.Booking.Leave_At.Month == month ||
                                                                    (b.Booking.Book_From.Month < month && b.Booking.Leave_At.Month > month)).Select(k=>k.Booking.state)).ToListAsync();

            return bookingRoom;
            
        }

        public async Task<Room> GetRoomById(int id)
        {
            var room = await _hotelDbContext.Room.Include(c => c.BookedRooms).ThenInclude(cs => cs.Booking).ThenInclude(st=>st.state).FirstOrDefaultAsync(d => d.Id == id);

            if(room == null)
            {
                return null;
            }

            return room;
        }

        public async Task<Room> UpdateRoomAsync(Room room)
        {
            _hotelDbContext.Room.Update(room);
            await _hotelDbContext.SaveChangesAsync();

            return room;
        }
    }
}
