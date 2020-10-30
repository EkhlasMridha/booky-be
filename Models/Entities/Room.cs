using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class Room
    {
        public Room()
        {
            BookedRooms = new HashSet<BookedRoom>();
        }
        public int Id { get; set; }
        public string Roomname { get; set; }
        public int Capacity { get; set; }

        public virtual ICollection<BookedRoom> BookedRooms { get; set; }
    }
}
