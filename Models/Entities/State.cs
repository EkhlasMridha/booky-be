using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class State
    {
        public State()
        {
            booking = new HashSet<Booking>();
        }
        public int Id { get; set; }
        public string Statename { get; set; }

        public virtual ICollection<Booking> booking { get; set; }
    }
}
