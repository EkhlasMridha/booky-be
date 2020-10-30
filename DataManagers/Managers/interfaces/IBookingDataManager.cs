using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.Dtos;

namespace DataManagers.Managers.interfaces
{
    public interface IBookingDataManager
    {
        public Task ChangeBookingState(State state);
        public Task DeleteBooking(Booking booking);
        public Task DeleteBookedRoom(BookedRoom bookedRoom);
        public Task<Booking> UpdateBooking(Booking booking);
        public Task<List<State>> GetAllStates();
        public Task<Booking> CreateBooking(Booking booking);
        public Task<Booking> GetBookingById(int id);
        public Task<bool> CheckBooking(int roomId, DateTime bookFrom, DateTime bookTo, int bookingId=0);
        public Task<Booking> CalculateTax(Booking booking,int userID);
        public Task<TaxContract> Settax(TaxContract taxContract);
        public Task<TaxContract> GetBookingTax(int id);
        public Task<IEnumerable<Guest>> GetGuestByBookingId(int bookingId);
    }
}
