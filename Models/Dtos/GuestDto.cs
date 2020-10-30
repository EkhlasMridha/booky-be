using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Dtos
{
    public class GuestDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
