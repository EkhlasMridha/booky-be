using DataManagers.Managers.interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManagers.Managers
{
    class BookingDataManager : IBookingDataManager
    {
        private HotelDbContext _hotelDbContext;
        public BookingDataManager(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }

        public async Task<Booking> CalculateTax(Booking booking, int userID)
        {
            if (booking.Guest.Count == 0)
            {
                return booking;
            }
            var tax = await _hotelDbContext.TaxContract.SingleOrDefaultAsync(t => t.UserId == userID);
            if (tax != null)
            {
                booking.Tax = 0;

                foreach(var guest in booking.Guest)
                {
                    if (guest.Age > 15)
                    {
                        booking.Tax += tax.AdultsTax;
                    }else if(guest.Age>=10 && guest.Age <= 15)
                    {
                        booking.Tax += tax.ChildrensTax;
                    }
                }
            }

            return booking;

        }

        public async Task ChangeBookingState(State state)
        {
            _hotelDbContext.State.Update(state);
            await _hotelDbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckBooking(int roomId,DateTime bookFrom,DateTime bookTo,int bookingId=0)
        {
            var fromDate = bookFrom;
            var toDate = bookTo;
            var status = await _hotelDbContext.BookedRoom.Where(a => a.RoomId == roomId)
                .Select(b => b.Booking)
                .Where(c => (c.Book_From == fromDate || (c.Book_From < toDate && c.Leave_At > fromDate)) && c.Id !=bookingId)
                .AnyAsync();

            return status;
        }

        public async Task<Booking> CreateBooking(Booking booking)
        {
            bool isExist = false;
            foreach (var bookedRoom in booking.BookedRoom)
            {
              bool check = await CheckBooking(bookedRoom.RoomId, booking.Book_From, booking.Leave_At);
                if (check)
                {
                    isExist = true;
                }
            }

            if (isExist)
            {
                return null;
            }

            await _hotelDbContext.Booking.AddAsync(booking);
           
            await _hotelDbContext.SaveChangesAsync();

            var state =  await _hotelDbContext.State.FirstOrDefaultAsync(s => s.Id == booking.StateId);
            booking.state = state;

            return booking;
        }

        public async Task DeleteBookedRoom(BookedRoom bookedRoom)
        {
            _hotelDbContext.BookedRoom.Remove(bookedRoom);
            await _hotelDbContext.SaveChangesAsync();
        }

        public async Task DeleteBooking(Booking booking)
        {
            _hotelDbContext.Booking.Remove(booking);
            await _hotelDbContext.SaveChangesAsync();
        }

        public async Task<List<State>> GetAllStates()
        {
            var stateList = await _hotelDbContext.State.ToListAsync();
            return stateList;
        }

        public async Task<Booking> GetBookingById(int id)
        {
           var booking = await _hotelDbContext.Booking.FirstOrDefaultAsync(b => b.Id == id);
            return booking;
        }

        public async Task<TaxContract> GetBookingTax(int id)
        {
            var tax = await _hotelDbContext.TaxContract.FirstOrDefaultAsync(t => t.UserId == id);
            return tax;
        }

        public async Task<IEnumerable<Guest>> GetGuestByBookingId(int bookingId)
        {
            var guest = await _hotelDbContext.Guest.Where(b => b.BookingId == bookingId).ToListAsync();
            return guest;
        }

        public async Task<TaxContract> Settax(TaxContract taxContract)
        {
            if(taxContract.Id == 0)
            {
                await _hotelDbContext.AddAsync(taxContract);
            }
            else
            {
                _hotelDbContext.Update(taxContract);
            }
            await _hotelDbContext.SaveChangesAsync();

            return taxContract;
        }

        public async Task<Booking> UpdateBooking(Booking booking)
        {
            if(booking.Guest.Count != 0)
            {
                _hotelDbContext.Guest.RemoveRange(_hotelDbContext.Guest.Where(g => g.BookingId == booking.Id));
            }
            _hotelDbContext.Booking.Update(booking);
            await _hotelDbContext.SaveChangesAsync();

            return booking;
        }

        
    }
}
