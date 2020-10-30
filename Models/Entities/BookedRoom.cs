using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class BookedRoom
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
