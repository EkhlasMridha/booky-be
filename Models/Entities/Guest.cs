using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class Guest
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
