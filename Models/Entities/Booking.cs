using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class Booking
    {
        public Booking()
        {
            BookedRoom = new HashSet<BookedRoom>();
            Guest = new HashSet<Guest>();
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

        public virtual State state { get; set; }
        public virtual ICollection<BookedRoom> BookedRoom { get; set; }
        public virtual ICollection<Guest> Guest { get; set; }
    }
}
