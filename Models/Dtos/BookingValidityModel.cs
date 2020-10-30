using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Dtos
{
    public class BookingValidityModel
    {
        public int BookingId{get;set;}
        public int roomId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
