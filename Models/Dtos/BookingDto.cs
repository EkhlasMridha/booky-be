using System;
using System.Collections.Generic;
using System.Text;
using Models.Entities;

namespace Models.Dtos
{
    public class BookingDto
    {
        public BookingDto()
        {
            BookedRoom = new HashSet<BookedRoomDto>();
            Guest = new HashSet<GuestDto>();
        }
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public DateTime Book_From { get; set; }
        public DateTime Leave_At { get; set; }
        public DateTime Booked_Date { get; set; }
        public double Amount { get; set; }
        public int StateId { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public double Tax { get; set; }
        public string Info { get; set; }

        public virtual ICollection<BookedRoomDto> BookedRoom { get; set; }
        public virtual ICollection<GuestDto> Guest { get; set; }
    }
}
